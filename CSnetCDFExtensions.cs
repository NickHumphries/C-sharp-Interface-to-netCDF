/*
 *  Use this file, rather than CsNetCDF.cs, for customised methods, such as multidimensional array
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
            return sb.ToString();
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

        #region Higher level methods
        // These methods wrap up some NetCDF function calls that make them easier to use
        //  but less robust - they will be fine if we know we are using a good NetCDF file or we're happy to skip some error processing
        // Get a global attribute
        public static string GetGlobalAttributeX(int ncid, string p_AttName)
        {
            try
            {
                if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out int len) != 0) throw new Exception("Global attribute " + p_AttName + " not found");
                if (type != nc_type.NC_STRING) throw new Exception("Global attribute " + p_AttName + " is not type NC_STRING");
                StringBuilder sb = new StringBuilder(len);
                if (nc_get_att_text(ncid, NC_GLOBAL, p_AttName, sb) != 0) return string.Empty;
                return sb.ToString().Substring(0, len);
            }
            catch (Exception e) { throw new Exception("GetGlobalAttribute for " + p_AttName + " failed", e); }
        }
        public static string GetGlobalAttribute(int ncid, string p_AttName)
        {
            try
            {
                if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out int len) != 0) return string.Empty;
                if (type != nc_type.NC_STRING && type != nc_type.NC_CHAR) return string.Empty;
                StringBuilder sb = new StringBuilder(len);
                if (nc_get_att_text(ncid, NC_GLOBAL, p_AttName, sb) != 0) return string.Empty;
                return sb.ToString().Substring(0, len);
            }
            catch (Exception e) { throw new Exception("GetGlobalAttribute for " + p_AttName + " failed", e); }
        }        public static bool GetGlobalAttribute(int ncid, string p_AttName, out int value)
        {
            value = 0;

            if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out int len) != 0) return false;

            int[] s = new int[len];

            if (nc_get_att_int(ncid, NC_GLOBAL, p_AttName, s) != 0) return false;
            value = s[0];
            return true;
        }

        public static double GetGlobalDouble(int ncid, string p_AttName)
        {
            if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out int len) != 0) return 0;
            double[] data = new double[len];
            if (nc_get_att_double(ncid, NC_GLOBAL, p_AttName, data) != 0) return 0;
            return data[0];
        }
        public static float GetGlobalFloat(int ncid, string p_AttName)
        {
            if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out int len) != 0) return 0;
            float[] data = new float[len];
            if (nc_get_att_float(ncid, NC_GLOBAL, p_AttName, data) != 0) return 0;
            return data[0];
        }

        public static short GetGlobalShort(int ncid, string p_AttName)
        {
            if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out int len) != 0) return 0;
            short[] data = new short[len];
            if (nc_get_att_short(ncid, NC_GLOBAL, p_AttName, data) != 0) return 0;
            return data[0];
        }

        public static int GetGlobalInt(int ncid, string p_AttName)
        {
            if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out int len) != 0) return 0;
            int[] data = new int[len];
            if (nc_get_att_int(ncid, NC_GLOBAL, p_AttName, data) != 0) return 0;
            return data[0];
        }

        public static bool GetGlobalBool(int ncid, string p_AttName)
        {
            if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out int len) != 0) return false;
            StringBuilder sb = new StringBuilder(len);
            if (nc_get_att_text(ncid, NC_GLOBAL, p_AttName, sb) != 0) return false;
            bool.TryParse(sb.ToString(), out bool result);
            return result;
        }

        // Dates are stored as strings in the metadata - more accessible
        public static DateTime GetGlobalDateTime(int ncid, string p_AttName)
        {
            if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out int len) != 0) return new DateTime();
            StringBuilder sb = new StringBuilder(len);
            if (nc_get_att_text(ncid, NC_GLOBAL, p_AttName, sb) != 0) return new DateTime();

            DateTime.TryParse(sb.ToString(), out DateTime date);
            return date;
        }

        // Get a variable attribute
        public static string GetVarAttribute(int ncid, string VarName, string p_AttName)
        {
            if (nc_inq_varid(ncid, VarName, out int varid) != 0) return string.Empty;
            return GetVarAttribute(ncid, varid, p_AttName);
        }

        public static string GetVarAttribute(int ncid, int varid, string p_AttName)
        {
            if (nc_inq_att(ncid, varid, p_AttName, out nc_type type, out int len) != 0) return string.Empty;
            StringBuilder sb = new StringBuilder(len);
            if (nc_get_att_text(ncid, varid, p_AttName, sb) != 0) return string.Empty;
            return sb.ToString().Substring(0, len);
        }
        public static bool GetVarAttribute(int ncid, string VarName, string p_AttName, out short value)
        {
            value = 0;

            if (nc_inq_varid(ncid, VarName, out int varid) != 0) return false;
            if (nc_inq_att(ncid, varid, p_AttName, out nc_type type, out int len) != 0) return false;

            short[] s = new short[len];

            if (nc_get_att_short(ncid, varid, p_AttName, s) != 0) return false;
            value = s[0];
            return true;
        }

        public static bool GetVarAttribute(int ncid, string VarName, string p_AttName, out int value)
        {
            value = 0;

            if (nc_inq_varid(ncid, VarName, out int varid) != 0) return false;
            if (nc_inq_att(ncid, varid, p_AttName, out nc_type type, out int len) != 0) return false;

            int[] s = new int[len];

            if (nc_get_att_int(ncid, varid, p_AttName, s) != 0) return false;
            value = s[0];
            return true;
        }

        public static bool GetVarAttribute(int ncid, string VarName, string p_AttName, out float value)
        {
            value = 0;

            if (nc_inq_varid(ncid, VarName, out int varid) != 0) return false;
            if (nc_inq_att(ncid, varid, p_AttName, out nc_type type, out int len) != 0) return false;

            float[] s = new float[len];

            if (nc_get_att_float(ncid, varid, p_AttName, s) != 0) return false;
            value = s[0];
            return true;
        }

        // Check if a variable exists
        public static bool VarExists(int ncid, string VarName)
        {
            return nc_inq_varid(ncid, VarName, out int varid) == 0;
        }

        // Get int data
        public static void Get_int(int ncid, string VarName, int[] data)
        {
            nc_inq_varid(ncid, VarName, out int varid);
            nc_get_var_int(ncid, varid, data);
        }

        // Get float data
        public static void Get_float(int ncid, string VarName, float[] data)
        {
            nc_inq_varid(ncid, VarName, out int varid);
            nc_get_var_float(ncid, varid, data);
        }

        public static float[] Get_float(int ncid, string VarName)
        {
            nc_inq_varid(ncid, VarName, out int varid);
            nc_inq_dimid(ncid, VarName, out int dimid);
            nc_inq_dimlen(ncid, dimid, out int PointsCount);
            float[] data = new float[PointsCount];
            nc_get_var_float(ncid, varid, data);
            return data;
        }

        // Get float data
        public static void Get_float(int ncid, string VarName, float[,,] data)
        {
            nc_inq_varid(ncid, VarName, out int varid);
            nc_get_var_float(ncid, varid, data);
        }

        // Get double data
        public static void Get_double(int ncid, string VarName, double[] data)
        {
            nc_inq_varid(ncid, VarName, out int varid);
            nc_get_var_double(ncid, varid, data);
        }

        // Get short data
        public static void Get_short(int ncid, string VarName, short[] data)
        {
            nc_inq_varid(ncid, VarName, out int varid);
            nc_get_var_short(ncid, varid, data);
        }

        public static void Get_short(int ncid, string VarName, short[,] data)
        {
            nc_inq_varid(ncid, VarName, out int varid);
            nc_get_var_short(ncid, varid, data);
        }

        // Get long data
        public static void Get_long(int ncid, string VarName, long[] data)
        {
            nc_inq_varid(ncid, VarName, out int varid);
            nc_get_var_longlong(ncid, varid, data);
        }

        // Get byte data
        public static void Get_byte(int ncid, string VarName, byte[] data)
        {
            nc_inq_varid(ncid, VarName, out int varid);
            nc_get_var_ubyte(ncid, varid, data);
        }

        // Methods to write attribute data
        public static void PutGlobalAttribute(int ncid, string AttName, string AttValue)
        {
            nc_put_att_text(ncid, NC_GLOBAL, AttName, AttValue.Length, AttValue);
        }

        public static void PutGlobalAttribute(int ncid, string AttName, DateTime AttValue)
        {
            string date = string.Empty;

            // Check for Access null dates and write an empty string
            date = AttValue.ToString("o");

            nc_put_att_text(ncid, NC_GLOBAL, AttName, date.Length, date);
        }

        public static void PutGlobalAttribute(int ncid, string AttName, double AttValue)
        {
            double[] att = new double[1];
            att[0] = AttValue;
            nc_put_att_double(ncid, NC_GLOBAL, AttName, nc_type.NC_DOUBLE, att.Length, att);
        }

        public static void PutGlobalAttribute(int ncid, string AttName, float AttValue)
        {
            float[] att = new float[1];
            att[0] = AttValue;
            nc_put_att_float(ncid, NC_GLOBAL, AttName, nc_type.NC_FLOAT, att.Length, att);
        }
        public static void PutGlobalAttribute(int ncid, string AttName, int AttValue)
        {
            int[] att = new int[1];
            att[0] = AttValue;
            nc_put_att_int(ncid, NC_GLOBAL, AttName, nc_type.NC_INT, att.Length, att);
        }

        public static void PutGlobalAttribute(int ncid, string AttName, bool AttValue)
        {
            PutGlobalAttribute(ncid, AttName, AttValue.ToString());
        }

        public static void PutVarAttribute(int ncid, int varid, string AttName, string AttValue)
        {
            nc_put_att_text(ncid, varid, AttName, AttValue.Length, AttValue);
        }

        public static void PutVarAttribute(int ncid, int varid, string AttName, DateTime AttValue)
        {
            string date = string.Empty;

            // Check for Access null dates and write an empty string
            date = AttValue.ToString("o");

            nc_put_att_text(ncid, varid, AttName, date.Length, date);
        }

        public static void PutVarAttribute(int ncid, int varid, string AttName, double AttValue)
        {
            double[] att = new double[1];
            att[0] = AttValue;
            nc_put_att_double(ncid, varid, AttName, nc_type.NC_DOUBLE, att.Length, att);
        }

        public static void PutVarAttribute(int ncid, int varid, string AttName, float AttValue)
        {
            float[] att = new float[1];
            att[0] = AttValue;
            nc_put_att_float(ncid, varid, AttName, nc_type.NC_FLOAT, att.Length, att);
        }
        public static void PutVarAttribute(int ncid, int varid, string AttName, int AttValue)
        {
            int[] att = new int[1];
            att[0] = AttValue;
            nc_put_att_int(ncid, varid, AttName, nc_type.NC_INT, att.Length, att);
        }

        public static void PutVarAttribute(int ncid, int varid, string AttName, bool AttValue)
        {
            PutVarAttribute(ncid, varid, AttName, AttValue.ToString());
        }

        #endregion

        #region Multi-dimensional array support
        // Get methods
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

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_short(int ncid, int varid, long[,] start, long[,] count, short[,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_short(int ncid, int varid, long[] start, long[] count, short[,,] ip);

        // Put methods
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var_float(int ncid, int varid, float[,,] op);

        #endregion
    }
}
