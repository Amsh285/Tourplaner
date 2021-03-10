using System;
using System.IO;
using System.Text.Json;

namespace Tourplaner.Infrastructure.Configuration
{
    public sealed class TourplanerConfigReader
    {
        public TourplanerConfig GetTourplanerConfig()
        {
            const string fileName = "TourplanerConfig.json";

            try
            {
                using (StreamReader reader = File.OpenText(fileName))
                {
                    return JsonSerializer.Deserialize<TourplanerConfig>(reader.ReadToEnd());
                }
            }
            catch(Exception ex) when (ex is FileNotFoundException notFoundEx)
            {
                throw new TourplanerConfigReaderException(
                    $"Die Konfigurationsdatei: {fileName} wurde nicht gefunden.",
                    notFoundEx
                );
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException unauthorizedEx)
            {
                throw new TourplanerConfigReaderException(
                    $"Zugriff auf die Konfigurationsdate: {fileName} wurde verweigert.",
                    unauthorizedEx
                );
            }
            catch(Exception ex) when (ex is JsonException jsonEx)
            {
                throw new TourplanerConfigReaderException(
                    $"Die Konfigurationsdate: {fileName} ist ungültig.",
                    jsonEx
                );
            }
        }
    }
}
