using System;

namespace VAICOM.Helpers
{
    public struct GeoCoordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public static class MapCoordinateConverter
    {
        // WGS84 ellipsoid
        private const double SemiMajorAxis = 6378137.0;
        private const double Flattening = 1.0 / 298.257223563;
        private const double EccentricitySquared = Flattening * (2.0 - Flattening);

        public static bool TryConvertDcsXYToLatLon(string theatre, double dcsX, double dcsY, out GeoCoordinate coordinate)
        {
            coordinate = default(GeoCoordinate);

            if (!MapProjectionCatalog.TryGetProjection(theatre, out TransverseMercatorProjection projection))
            {
                return false;
            }

            try
            {
                // DCS XY ordering is converted as (easting = y, northing = x)
                // to align with existing projection validation vectors.
                double easting = dcsY;
                double northing = dcsX;

                coordinate = InverseTransverseMercator(easting, northing, projection);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static GeoCoordinate InverseTransverseMercator(double easting, double northing, TransverseMercatorProjection projection)
        {
            double x = easting - projection.FalseEastingMeters;
            double y = northing - projection.FalseNorthingMeters;

            double ePrimeSquared = EccentricitySquared / (1.0 - EccentricitySquared);

            double m = y / projection.ScaleFactor;
            double mu = m / (SemiMajorAxis * (1.0
                - (EccentricitySquared / 4.0)
                - (3.0 * Math.Pow(EccentricitySquared, 2.0) / 64.0)
                - (5.0 * Math.Pow(EccentricitySquared, 3.0) / 256.0)));

            double e1 = (1.0 - Math.Sqrt(1.0 - EccentricitySquared)) / (1.0 + Math.Sqrt(1.0 - EccentricitySquared));

            double j1 = (3.0 * e1 / 2.0) - (27.0 * Math.Pow(e1, 3.0) / 32.0);
            double j2 = (21.0 * Math.Pow(e1, 2.0) / 16.0) - (55.0 * Math.Pow(e1, 4.0) / 32.0);
            double j3 = (151.0 * Math.Pow(e1, 3.0) / 96.0);
            double j4 = (1097.0 * Math.Pow(e1, 4.0) / 512.0);

            double fp = mu
                + j1 * Math.Sin(2.0 * mu)
                + j2 * Math.Sin(4.0 * mu)
                + j3 * Math.Sin(6.0 * mu)
                + j4 * Math.Sin(8.0 * mu);

            double sinFp = Math.Sin(fp);
            double cosFp = Math.Cos(fp);
            double tanFp = Math.Tan(fp);

            double c1 = ePrimeSquared * Math.Pow(cosFp, 2.0);
            double t1 = Math.Pow(tanFp, 2.0);
            double n1 = SemiMajorAxis / Math.Sqrt(1.0 - EccentricitySquared * Math.Pow(sinFp, 2.0));
            double r1 = (SemiMajorAxis * (1.0 - EccentricitySquared))
                / Math.Pow(1.0 - EccentricitySquared * Math.Pow(sinFp, 2.0), 1.5);
            double d = x / (n1 * projection.ScaleFactor);

            double latRad = fp - (n1 * tanFp / r1)
                * ((Math.Pow(d, 2.0) / 2.0)
                   - ((5.0 + 3.0 * t1 + 10.0 * c1 - 4.0 * Math.Pow(c1, 2.0) - 9.0 * ePrimeSquared) * Math.Pow(d, 4.0) / 24.0)
                   + ((61.0 + 90.0 * t1 + 298.0 * c1 + 45.0 * Math.Pow(t1, 2.0) - 252.0 * ePrimeSquared - 3.0 * Math.Pow(c1, 2.0)) * Math.Pow(d, 6.0) / 720.0));

            double lon0Rad = DegreesToRadians(projection.CentralMeridianDeg);
            double lonRad = lon0Rad
                + ((d
                    - (1.0 + 2.0 * t1 + c1) * Math.Pow(d, 3.0) / 6.0
                    + (5.0 - 2.0 * c1 + 28.0 * t1 - 3.0 * Math.Pow(c1, 2.0) + 8.0 * ePrimeSquared + 24.0 * Math.Pow(t1, 2.0)) * Math.Pow(d, 5.0) / 120.0)
                   / cosFp);

            return new GeoCoordinate
            {
                Latitude = RadiansToDegrees(latRad),
                Longitude = RadiansToDegrees(lonRad),
            };
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        private static double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }
    }
}
