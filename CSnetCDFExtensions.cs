/*
 *  Use this file, rather than CSnetCDF.cs, for customised methods, such as multidimensional array
 *  support, or other helper methods.
 */
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CsNetCDF
{
    public static partial class NetCDF
    {
        #region C# Friendly methods
        //
        // C# Friendly methods for returning strings
        //  These methods return the status as a parameter
        //
        /// <summary>Friendly method for nc_inq_path</summary>
        public static string nc_inq_path(int ncid, out int status)
        {
            status = nc_inq_path(ncid, out int len, null);
            if (status != 0) return string.Empty;
            StringBuilder sb = new StringBuilder(len);
            status = nc_inq_path(ncid, out len, sb);
            if (status != 0) return string.Empty;
            return sb.ToString();
        }

        /// <summary>Friendly method for nc_inq_grpname_full</summary>
        public static string nc_inq_grpname_full(int ncid, out int status)
        {
            status = nc_inq_grpname_full(ncid, out int len, null);
            if (status != 0) return string.Empty;
            StringBuilder sb = new StringBuilder(len);
            status = nc_inq_grpname_full(ncid, out len, sb);
            if (status != 0) return string.Empty;
            return sb.ToString();
        }

        //
        // Friendly methods for reading attributes
        //
        public static string nc_get_att_text(int ncid, int varid, string name, out int status)
        {
            status = nc_inq_attlen(ncid, varid, name, out int lenp);
            if (status != 0) return string.Empty;
            StringBuilder sb = new StringBuilder(lenp);
            status = nc_get_att_text(ncid, varid, name, sb);
            if (status != 0) return string.Empty;
            
            // We perform a substring here to remove the null terminator
            return sb.ToString().Substring(0, lenp);
        }
        public static sbyte[] nc_get_att_schar(int ncid, int varid, string name, out int status)
        {
            status = nc_inq_attlen(ncid, varid, name, out int lenp);
            if (status != 0) return null;
            sbyte[] value = new sbyte[lenp];
            status = nc_get_att_schar(ncid, varid, name, value);
            if (status != 0) return null;
            return value;
        }
        public static byte[] nc_get_att_uchar(int ncid, int varid, string name, out int status)
        {
            status = nc_inq_attlen(ncid, varid, name, out int lenp);
            if (status != 0) return null;
            byte[] value = new byte[lenp];
            status = nc_get_att_uchar(ncid, varid, name, value);
            if (status != 0) return null;
            return value;
        }
        public static short[] nc_get_att_short(int ncid, int varid, string name, out int status)
        {
            status = nc_inq_attlen(ncid, varid, name, out int lenp);
            if (status != 0) return null;
            short[] value = new short[lenp];
            status = nc_get_att_short(ncid, varid, name, value);
            if (status != 0) return null;
            return value;
        }
        public static int[] nc_get_att_int(int ncid, int varid, string name, out int status)
        {
            status = nc_inq_attlen(ncid, varid, name, out int lenp);
            if (status != 0) return null;
            int[] value = new int[lenp];
            status = nc_get_att_int(ncid, varid, name, value);
            if (status != 0) return null;
            return value;
        }
        public static long[] nc_get_att_long(int ncid, int varid, string name, out int status)
        {
            status = nc_inq_attlen(ncid, varid, name, out int lenp);
            if (status != 0) return null;
            long[] value = new long[lenp];
            status = nc_get_att_long(ncid, varid, name, value);
            if (status != 0) return null;
            return value;
        }
        public static float[] nc_get_att_float(int ncid, int varid, string name, out int status)
        {
            status = nc_inq_attlen(ncid, varid, name, out int lenp);
            if (status != 0) return null;
            float[] value = new float[lenp];
            status = nc_get_att_float(ncid, varid, name, value);
            if (status != 0) return null;
            return value;
        }
        public static double[] nc_get_att_double(int ncid, int varid, string name, out int status)
        {
            status = nc_inq_attlen(ncid, varid, name, out int lenp);
            if (status != 0) return null;
            double[] value = new double[lenp];
            status = nc_get_att_double(ncid, varid, name, value);
            if (status != 0) return null;
            return value;
        }
        public static byte[] nc_get_att_ubyte(int ncid, int varid, string name, out int status)
        {
            status = nc_inq_att(ncid, varid, name, out nc_type nctype, out int lenp);
            if (status != 0) return null;
            byte[] value = new byte[lenp];
            status = nc_get_att_ubyte(ncid, varid, name, value);
            if (status != 0) return null;
            return value;
        }
        public static ushort[] nc_get_att_ushort(int ncid, int varid, string name, out int status)
        {
            status = nc_inq_attlen(ncid, varid, name, out int lenp);
            if (status != 0) return null;
            ushort[] value = new ushort[lenp];
            status = nc_get_att_ushort(ncid, varid, name, value);
            if (status != 0) return null;
            return value;
        }
        public static uint[] nc_get_att_uint(int ncid, int varid, string name, out int status)
        {
            status = nc_inq_attlen(ncid, varid, name, out int lenp);
            if (status != 0) return null;
            uint[] value = new uint[lenp];
            status = nc_get_att_uint(ncid, varid, name, value);
            if (status != 0) return null;
            return value;
        }
        public static long[] nc_get_att_longlong(int ncid, int varid, string name, out int status)
        {
            status = nc_inq_attlen(ncid, varid, name, out int lenp);
            if (status != 0) return null;
            long[] value = new long[lenp];
            status = nc_get_att_longlong(ncid, varid, name, value);
            if (status != 0) return null;
            return value;
        }
        public static ulong[] nc_get_att_ulonglong(int ncid, int varid, string name, out int status)
        {
            status = nc_inq_attlen(ncid, varid, name, out int lenp);
            if (status != 0) return null;
            ulong[] value = new ulong[lenp];
            status = nc_get_att_ulonglong(ncid, varid, name, value);
            if (status != 0) return null;
            return value;
        }

        //
        // Friendly methods for writing attributes
        //
        public static int nc_put_att_text(int ncid, int varid, string name, string value)
        {
            return nc_put_att_text(ncid, varid, name, value.Length, value);
        }

        public static int nc_put_att_double(int ncid, int varid, string name, double[] value)
        {
            return nc_put_att_double(ncid, varid, name, nc_type.NC_DOUBLE, value.Length, value);
        }

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_double(int ncid, int varid, string name, double value)
        {
            double[] v = new double[1];
            v[0] = value;
            return nc_put_att_double(ncid, varid, name, v);
        }

        public static int nc_put_att_float(int ncid, int varid, string name, float[] value)
        {
            return nc_put_att_float(ncid, varid, name, nc_type.NC_FLOAT, value.Length, value);
        }

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_float(int ncid, int varid, string name, float value)
        {
            float[] v = new float[1];
            v[0] = value;
            return nc_put_att_float(ncid, varid, name, v);
        }

        public static int nc_put_att_int(int ncid, int varid, string name, int[] value)
        {
            return nc_put_att_int(ncid, varid, name, nc_type.NC_INT, value.Length, value);
        }

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_int(int ncid, int varid, string name, int value)
        {
            int[] v = new int[1];
            v[0] = value;
            return nc_put_att_int(ncid, varid, name, v);
        }
        public static int nc_put_att_long(int ncid, int varid, string name, long[] value)
        {
            return nc_put_att_long(ncid, varid, name, nc_type.NC_INT64, value.Length, value);
        }

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_long(int ncid, int varid, string name, long value)
        {
            long[] v = new long[1];
            v[0] = value;
            return nc_put_att_long(ncid, varid, name, v);
        }

        public static int nc_put_att_longlong(int ncid, int varid, string name, long[] value)
        {
            return nc_put_att_longlong(ncid, varid, name, nc_type.NC_INT64, value.Length, value);
        }

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_longlong(int ncid, int varid, string name, long value)
        {
            long[] v = new long[1];
            v[0] = value;
            return nc_put_att_long(ncid, varid, name, v);
        }

        public static int nc_put_att_schar(int ncid, int varid, string name, sbyte[] value)
        {
            return nc_put_att_schar(ncid, varid, name, nc_type.NC_BYTE, value.Length, value);
        }

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_schar(int ncid, int varid, string name, sbyte value)
        {
            sbyte[] v = new sbyte[1];
            v[0] = value;
            return nc_put_att_schar(ncid, varid, name, v);
        }

        public static int nc_put_att_short(int ncid, int varid, string name, short[] value)
        {
            return nc_put_att_short(ncid, varid, name, nc_type.NC_SHORT, value.Length, value);
        }

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_short(int ncid, int varid, string name, short value)
        {
            short[] v = new short[1];
            v[0] = value;
            return nc_put_att_short(ncid, varid, name, v);
        }

        public static int nc_put_att_ubyte(int ncid, int varid, string name, byte[] value)
        {
            return nc_put_att_ubyte(ncid, varid, name, nc_type.NC_UBYTE, value.Length, value);
        }

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_ubyte(int ncid, int varid, string name, byte value)
        {
            byte[] v = new byte[1];
            v[0] = value;
            return nc_put_att_ubyte(ncid, varid, name, v);
        }

        public static int nc_put_att_uchar(int ncid, int varid, string name, byte[] value)
        {
            return nc_put_att_uchar(ncid, varid, name, nc_type.NC_CHAR, value.Length, value);
        }

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_uchar(int ncid, int varid, string name, byte value)
        {
            byte[] v = new byte[1];
            v[0] = value;
            return nc_put_att_uchar(ncid, varid, name, v);
        }

        public static int nc_put_att_uint(int ncid, int varid, string name, uint[] value)
        {
            return nc_put_att_uint(ncid, varid, name, nc_type.NC_UINT, value.Length, value);
        }

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_uint(int ncid, int varid, string name, uint value)
        {
            uint[] v = new uint[1];
            v[0] = value;
            return nc_put_att_uint(ncid, varid, name, v);
        }

        public static int nc_put_att_ulonglong(int ncid, int varid, string name, ulong[] value)
        {
            return nc_put_att_ulonglong(ncid, varid, name, nc_type.NC_UINT64, value.Length, value);
        }

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_ulonglong(int ncid, int varid, string name, ulong value)
        {
            ulong[] v = new ulong[1];
            v[0] = value;
            return nc_put_att_ulonglong(ncid, varid, name, v);
        }

        public static int nc_put_att_ushort(int ncid, int varid, string name, ushort[] value)
        {
            return nc_put_att_ushort(ncid, varid, name, nc_type.NC_USHORT, value.Length, value);
        }

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_ushort(int ncid, int varid, string name, ushort value)
        {
            ushort[] v = new ushort[1];
            v[0] = value;
            return nc_put_att_ushort(ncid, varid, name, v);
        }

        //
        //  Friendly methods for dealing with strings and other awkward data types
        //
        // Methods for strings
        public static string[] nc_get_var_string(int ncid, int varid, out int status)
        {
            int[] dim = new int[1];
            status = nc_inq_vardimid(ncid, varid, dim);
            if (status != 0) return null;
            status = nc_inq_dimlen(ncid, dim[0], out int length);
            if (status != 0) return null;
            IntPtr[] ptrs = new IntPtr[length];
            status = nc_get_var_string(ncid, varid, ptrs);
            if (status != 0) return null;
            string[] s = new string[ptrs.Length];
            for (int i = 0; i < ptrs.Length; i++) s[i] = Marshal.PtrToStringAnsi(ptrs[i]);
            status = nc_free_string((IntPtr)ptrs.Length, ptrs);
            return s;
        }

        public static string nc_get_var1_string(int ncid, int varid, int[] index, out int status)
        {
            IntPtr[] ptrs = new IntPtr[1];
            status = nc_get_var1_string(ncid, varid, index, ptrs);
            if (status != 0) return null;
            string s;
            s = Marshal.PtrToStringAnsi(ptrs[0]);
            status = nc_free_string((IntPtr)ptrs.Length, ptrs);
            return s;
        }

        public static string[] nc_get_vara_string(int ncid, int varid, int[] start, int[] count, out int status)
        {
            int len = 0;
            for (int i = 0; i < count.Length; i++) len += count[i];
            IntPtr[] ptrs = new IntPtr[len];
            status = nc_get_vara_string(ncid, varid, start, count, ptrs);
            if (status != 0) return null;
            string[] s = new string[len];
            for (int i = 0; i < ptrs.Length; i++) s[i] = Marshal.PtrToStringAnsi(ptrs[i]);
            status = nc_free_string((IntPtr)ptrs.Length, ptrs);
            return s;
        }

        public static string[] nc_get_vars_string(int ncid, int varid, int[] start, int[] count, int[] stride, out int status)
        {
            int len = 0;
            for (int i = 0; i < count.Length; i++) len += count[i] / stride[i];
            IntPtr[] ptrs = new IntPtr[len];
            status = nc_get_vars_string(ncid, varid, start, count, stride, ptrs);
            if (status != 0) return null;
            string[] s = new string[len];
            for (int i = 0; i < ptrs.Length; i++) s[i] = Marshal.PtrToStringAnsi(ptrs[i]);
            status = nc_free_string((IntPtr)ptrs.Length, ptrs);
            return s;
        }

        #endregion

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
