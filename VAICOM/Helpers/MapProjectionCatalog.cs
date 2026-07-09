using System;
using System.Collections.Generic;

namespace VAICOM.Helpers
{
    public struct TransverseMercatorProjection
    {
        public string Theater { get; set; }
        public double CentralMeridianDeg { get; set; }
        public double FalseEastingMeters { get; set; }
        public double FalseNorthingMeters { get; set; }
        public double ScaleFactor { get; set; }
    }

    public static class MapProjectionCatalog
    {
        private static readonly HashSet<string> KnownTheatres
            = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Caucasus",
                "Nevada",
                "Normandy",
                "PersianGulf",
                "TheChannel",
                "Syria",
                "MarianaIslands",
                "Falklands",
                "SinaiMap",
                "Kola",
                "Afghanistan",
                "Iraq",
                "GermanyCW",
            };

        private static readonly Dictionary<string, TransverseMercatorProjection> TheatreProjections
            = new Dictionary<string, TransverseMercatorProjection>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "PersianGulf",
                    new TransverseMercatorProjection
                    {
                        Theater = "PersianGulf",
                        CentralMeridianDeg = 57,
                        FalseEastingMeters = 75755.99999999645,
                        FalseNorthingMeters = -2894933.0000000377,
                        ScaleFactor = 0.9996,
                    }
                },
                {
                    "Falklands",
                    new TransverseMercatorProjection
                    {
                        Theater = "Falklands",
                        CentralMeridianDeg = -57,
                        FalseEastingMeters = 147639.99999997593,
                        FalseNorthingMeters = 5815417.000000032,
                        ScaleFactor = 0.9996,
                    }
                },
                {
                    "Caucasus",
                    new TransverseMercatorProjection
                    {
                        Theater = "Caucasus",
                        CentralMeridianDeg = 33,
                        FalseEastingMeters = -99516.99999997323,
                        FalseNorthingMeters = -4998114.999999984,
                        ScaleFactor = 0.9996,
                    }
                },
                {
                    "MarianaIslands",
                    new TransverseMercatorProjection
                    {
                        Theater = "MarianaIslands",
                        CentralMeridianDeg = 147,
                        FalseEastingMeters = 238417.999991766526,
                        FalseNorthingMeters = -1491839.999998347135,
                        ScaleFactor = 0.9996,
                    }
                },
                {
                    "Nevada",
                    new TransverseMercatorProjection
                    {
                        Theater = "Nevada",
                        CentralMeridianDeg = -117,
                        FalseEastingMeters = -193996.80999964548,
                        FalseNorthingMeters = -4410028.063999966,
                        ScaleFactor = 0.9996,
                    }
                },
                {
                    "Normandy",
                    new TransverseMercatorProjection
                    {
                        Theater = "Normandy",
                        CentralMeridianDeg = -3,
                        FalseEastingMeters = -195526.00000000204,
                        FalseNorthingMeters = -5484812.999999951,
                        ScaleFactor = 0.9996,
                    }
                },
                {
                    "Syria",
                    new TransverseMercatorProjection
                    {
                        Theater = "Syria",
                        CentralMeridianDeg = 39,
                        FalseEastingMeters = 282801.000060651742,
                        FalseNorthingMeters = -3879866.000066791195,
                        ScaleFactor = 0.9996,
                    }
                },
                {
                    "SinaiMap",
                    new TransverseMercatorProjection
                    {
                        Theater = "SinaiMap",
                        CentralMeridianDeg = 33,
                        FalseEastingMeters = 169221.9999999585,
                        FalseNorthingMeters = -3325312.9999999693,
                        ScaleFactor = 0.9996,
                    }
                },
                {
                    "TheChannel",
                    new TransverseMercatorProjection
                    {
                        Theater = "TheChannel",
                        CentralMeridianDeg = 21,
                        FalseEastingMeters = -62702,
                        FalseNorthingMeters = -7543624.99999998,
                        ScaleFactor = 0.9996,
                    }
                },
                {
                    "Afghanistan",
                    new TransverseMercatorProjection
                    {
                        Theater = "Afghanistan",
                        CentralMeridianDeg = 63,
                        FalseEastingMeters = -300150.032879,
                        FalseNorthingMeters = -3759656.99243,
                        ScaleFactor = 0.9996,
                    }
                },
                {
                    "Kola",
                    new TransverseMercatorProjection
                    {
                        Theater = "Kola",
                        CentralMeridianDeg = 21,
                        FalseEastingMeters = -62702.000001687862,
                        FalseNorthingMeters = -7543625.000940491445,
                        ScaleFactor = 0.9996,
                    }
                },
                {
                    "GermanyCW",
                    new TransverseMercatorProjection
                    {
                        Theater = "GermanyCW",
                        CentralMeridianDeg = 21,
                        FalseEastingMeters = 35427.62000191761,
                        FalseNorthingMeters = -6061633.128573619,
                        ScaleFactor = 0.9996,
                    }
                },
                {
                    "Iraq",
                    new TransverseMercatorProjection
                    {
                        Theater = "Iraq",
                        CentralMeridianDeg = 45,
                        FalseEastingMeters = 72289.999997470353,
                        FalseNorthingMeters = -3680057.000046721194,
                        ScaleFactor = 0.9996,
                    }
                },
            };

        public static bool TryGetProjection(string theatre, out TransverseMercatorProjection projection)
        {
            if (string.IsNullOrWhiteSpace(theatre))
            {
                projection = default(TransverseMercatorProjection);
                return false;
            }

            return TheatreProjections.TryGetValue(theatre.Trim(), out projection);
        }

        public static bool IsKnownTheatre(string theatre)
        {
            return !string.IsNullOrWhiteSpace(theatre) && KnownTheatres.Contains(theatre.Trim());
        }
    }
}
