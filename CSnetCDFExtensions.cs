/*
 *  Use this file, rather than CSnetCDF.cs, for customised methods, such as multidimensional array
 *  support, or other helper methods.
 */
using System.Runtime.InteropServices;

namespace CsNetCDF
{
    public static partial class NetCDF
    {
        #region Multi-dimensional array support example
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_float(int ncid, int varid, float[,,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_float(int ncid, int varid, long[] start, long[] count, float[,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_float(int ncid, int varid, long[] start, long[] count, float[,,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_short(int ncid, int varid, long[] start, long[] count, short[,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_short(int ncid, int varid, short[,] ip);


        #endregion
    }
}
