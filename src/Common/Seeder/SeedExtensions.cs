using Microsoft.Extensions.Hosting;
using Seeder.Models;
using Seeder.Models.FileEntity;
using System.Globalization;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Seeder
{
    public static class SeedExtensions
    {
        private static readonly IDeserializer _yamlDeserializer = new DeserializerBuilder()
                                                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                                                        .Build();

        public static IDeserializer GetDeserializer() => _yamlDeserializer;

        public static async Task<string?> GenerateScript(this SeedFile file, string basePath, IHostEnvironment env)
        {
            var splitted = file.Path.Split('/');
            var memberPath = Path.Combine(basePath, Path.Combine(splitted));

            if (!File.Exists(memberPath))
            {
                throw new FileNotFoundException($"YAML file not found: {memberPath}");
            }

            if (file.IsProcedure)
            {
                return await File.ReadAllTextAsync(memberPath);
            }

            SeedMember member;
            SeedMemberFile seedMemberFile;

            member = _yamlDeserializer.Deserialize<SeedMember>(await File.ReadAllTextAsync(memberPath, Encoding.UTF8));

            if (member.Metadata.IsDevTestOnly && (env.IsProduction() || env.IsStaging()))
            {
                return null;
            }

            if (member.Metadata.AdditionalUseCaseName != null && member.Metadata.AdditionalUseCaseName == "FilesUseCase")
            {
                seedMemberFile = _yamlDeserializer.Deserialize<SeedMemberFile>(await File.ReadAllTextAsync(memberPath, Encoding.UTF8));
                member = seedMemberFile.Metadata.ConvertToSeedMember(seedMemberFile.Data);
            }

            var sqlScript = member.GenerateInsertStatements();

            sqlScript = sqlScript
                .Replace("{", "{{", StringComparison.Ordinal)
                .Replace("}", "}}", StringComparison.Ordinal);

            return sqlScript;
        }

        private static SeedMember ConvertToSeedMember(this SeedMemberMetadata meta, List<SeedFileData>? data)
        {
            SeedFileData s;

            var result = new SeedMember
            {
                Metadata = meta,
                Data = data == null
                    ? []
                    : [.. data.Select(item =>
                {
                    var dict = new Dictionary<object, object?>();

                    var props = typeof(SeedFileData).GetProperties();

                    var metadataJsonProp = props.FirstOrDefault(p => p.Name == nameof(s.MetadataJson));
                    var metadataJsonValue = metadataJsonProp?.GetValue(item);

                    foreach (var prop in props)
                    {
                        if (prop.Name == nameof(s.MetadataJson))
                        {
                            continue;
                        }

                        if (prop.Name == nameof(s.Metadata))
                        {
                            dict[nameof(s.Metadata)] = metadataJsonValue;
                        }
                        else if (prop.Name == nameof(s.CreatedAt))
                        {
                            var createdAtValue = prop.GetValue(item);
                            if (createdAtValue != null)
                            {
                                string[] formats = [
                                    "dd.MM.yyyy H:mm:ss",
                                    "dd.MM.yyyy HH:mm:ss",
                                    "yyyy-MM-ddTHH:mm:ss",
                                    "yyyy-MM-dd HH:mm:ss",
                                    "M/d/yyyy h:mm:ss tt",
                                    "MM/dd/yyyy h:mm:ss tt",
                                    "M/d/yyyy hh:mm:ss tt",
                                    "MM/dd/yyyy HH:mm:ss",
                                ];

#pragma warning disable CS8604 // Possible null reference argument.
                                var dt = DateTime.ParseExact(
                                    createdAtValue.ToString(),
                                    formats,
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None);
#pragma warning restore CS8604 // Possible null reference argument.

                                dict[nameof(s.CreatedAt)] = dt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            }
                        }
                        else
                        {
                            dict[prop.Name] = prop.GetValue(item);
                        }
                    }

                    return (object)dict;
                })]
            };

            return result;
        }

        private static string GenerateInsertStatements(this SeedMember seedMember)
        {
            var metadata = seedMember.Metadata;
            var dataList = seedMember.Data;

            var sb = new StringBuilder();

            if (seedMember.Metadata.AllowIdentityInsert)
            {
                sb.AppendLine($"SET IDENTITY_INSERT {seedMember.Metadata.TableName} ON;".ToString(CultureInfo.InvariantCulture));
            }

            if (metadata.Force)
            {
                sb.AppendLine($"DELETE FROM {metadata.TableName};".ToString(CultureInfo.InvariantCulture));
            }

            sb.AppendLine($"""
            DECLARE @ColumnType NVARCHAR(50);
            SELECT @ColumnType = DATA_TYPE
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = '{metadata.TableName}' AND COLUMN_NAME = 'id';

            IF @ColumnType = 'bigint'
            BEGIN
                DBCC CHECKIDENT ('{metadata.TableName}', RESEED, 1);
            END
            """.ToString(CultureInfo.InvariantCulture));

            foreach (var data in dataList)
            {
                if (data is IDictionary<object, object> dictData)
                {
                    var columns = string.Join(", ", dictData.Keys.Select(k => k.ToString()));
                    var values = string.Join(", ", dictData.Values.Select(HandleValue));

                    var mainPair = dictData
                        .OrderBy(kvp => kvp.Key.ToString()!.Equals("id", StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                        .ThenBy(kvp => kvp.Key)
                        .FirstOrDefault(kvp =>
                            kvp.Key.ToString()!.Equals("id", StringComparison.OrdinalIgnoreCase) ||
                            kvp.Key.ToString()!.Equals("code", StringComparison.OrdinalIgnoreCase) ||
                            kvp.Key.ToString()!.Equals("translationCode", StringComparison.OrdinalIgnoreCase));

                    var key = mainPair.Key?.ToString();
                    var value = mainPair.Value;

                    var setData = dictData.Keys.Select(k => k.ToString())
                        .Zip(dictData.Values.Select(HandleValue));

                    var set = string.Join(", ", setData.Where(x => x.First != key).Select(x => $"{x.First} = {x.Second}"));

                    if (key != null && value != null)
                    {
                        sb.AppendLine($@"
IF NOT EXISTS (SELECT 1 FROM {metadata.TableName} WHERE {key} = {HandleValue(value)})
BEGIN
    INSERT INTO {metadata.TableName} ({columns}) VALUES ({values});
END".ToString(CultureInfo.InvariantCulture));
                        if (set.Length > 0)
                        {
                            sb.AppendLine($@"ELSE
BEGIN
    UPDATE {metadata.TableName}
    SET {set}
    WHERE {key} = {HandleValue(value)};
END".ToString(CultureInfo.InvariantCulture));
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("Data items must be either objects with properties or dictionaries.");
                }
            }

            if (seedMember.Metadata.AllowIdentityInsert)
            {
                sb.AppendLine($"SET IDENTITY_INSERT {seedMember.Metadata.TableName} OFF;".ToString(CultureInfo.InvariantCulture));
                sb.AppendLine($"DECLARE @{seedMember.Metadata.TableName}count INT = (SELECT ISNULL(MAX(Id), 0) FROM {seedMember.Metadata.TableName});".ToString(CultureInfo.InvariantCulture));
                sb.AppendLine($"DBCC CHECKIDENT ('{seedMember.Metadata.TableName}', RESEED, @{seedMember.Metadata.TableName}count);".ToString(CultureInfo.InvariantCulture));
            }

            return sb.ToString();
        }

        private static string HandleValue(object? var)
        {
            if (var is null)
            {
                return "NULL";
            }

            if (long.TryParse(var.ToString(), out _) && !var.ToString()!.Contains('.', StringComparison.InvariantCulture) && !var.ToString()!.StartsWith('0'))
            {
                return var.ToString()!;
            }

            var str = var.ToString();

            if (str == "true")
            {
                return "1";
            }

            if (str == "false")
            {
                return "0";
            }

            return $"N'{str!.Replace("'", "''", StringComparison.InvariantCulture)}'";
        }
    }
}
