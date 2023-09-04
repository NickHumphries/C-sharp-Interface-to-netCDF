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
        #region Wrapping the IntPtr calls
        #region Misc and attribute methods
        public static int nc_inq_path(int ncid, out int pathlen, StringBuilder path)
        {
            int status = nc_inq_path(ncid, out IntPtr len, path);
            pathlen = (int)len;
            return status;
        }
        public static int nc_inq_att(int ncid, int varid, string name, out nc_type xtypep, out int len)
        {
            int status = nc_inq_att(ncid, varid, name, out xtypep, out IntPtr lenp);
            len = (int)lenp;
            return status;
        }
        public static int nc_inq_attlen(int ncid, int varid, string name, out int len)
        {
            int status = nc_inq_attlen(ncid, varid, name, out IntPtr lenp);
            len = (int)lenp;
            return status;
        }
        public static int nc_put_att_text(int ncid, int varid, string name, int len, string value) => nc_put_att_text(ncid, varid, name, (IntPtr)len, value);
        public static int nc_put_att_schar(int ncid, int varid, string name, nc_type type, int len, sbyte[] value) => nc_put_att_schar(ncid, varid, name, type, (IntPtr)len, value);
        public static int nc_put_att_uchar(int ncid, int varid, string name, nc_type type, int len, byte[] value) => nc_put_att_uchar(ncid, varid, name, type, (IntPtr)len, value);
        public static int nc_put_att_short(int ncid, int varid, string name, nc_type type, int len, short[] value) => nc_put_att_short(ncid, varid, name, type, (IntPtr)len, value);
        public static int nc_put_att_int(int ncid, int varid, string name, nc_type type, int len, int[] value) => nc_put_att_int(ncid, varid, name, type, (IntPtr)len, value);
        public static int nc_put_att_long(int ncid, int varid, string name, nc_type type, int len, long[] value) => nc_put_att_long(ncid, varid, name, type, (IntPtr)len, value);
        public static int nc_put_att_float(int ncid, int varid, string name, nc_type type, int len, float[] value) => nc_put_att_float(ncid, varid, name, type, (IntPtr)len, value);
        public static int nc_put_att_double(int ncid, int varid, string name, nc_type type, int len, double[] value) => nc_put_att_double(ncid, varid, name, type, (IntPtr)len, value);
        public static int nc_put_att_ubyte(int ncid, int varid, string name, nc_type type, int len, byte[] value) => nc_put_att_ubyte(ncid, varid, name, type, (IntPtr)len, value);
        public static int nc_put_att_ushort(int ncid, int varid, string name, nc_type type, int len, ushort[] value) => nc_put_att_ushort(ncid, varid, name, type, (IntPtr)len, value);
        public static int nc_put_att_uint(int ncid, int varid, string name, nc_type type, int len, uint[] value) => nc_put_att_uint(ncid, varid, name, type, (IntPtr)len, value);
        public static int nc_put_att_ulonglong(int ncid, int varid, string name, nc_type type, int len, ulong[] value) => nc_put_att_ulonglong(ncid, varid, name, type, (IntPtr)len, value);
        public static int nc_inq_grpname_full(int ncid, out int len, StringBuilder full_name)
        {
            int status = nc_inq_grpname_full(ncid, out IntPtr lenp, full_name);
            len = (int)lenp;
            return status;
        }
        public static int nc_inq_grpname_len(int ncid, out int len)
        {
            int status = nc_inq_grpname_len(ncid, out IntPtr lenp);
            len = (int)lenp;
            return status;
        }
        public static int nc_get_vlen_element(int ncid, int typeid1, object vlen_element, out int len, out object data)
        {
            int status = nc_get_vlen_element(ncid, typeid1, vlen_element, out IntPtr lenp, out data);
            len = (int)lenp;
            return status;
        }
        public static int nc_def_dim(int ncid, string name, int len, out int dimidp) => nc_def_dim(ncid, name, (IntPtr)len, out dimidp);
        public static int nc_inq_dim(int ncid, int dimid, StringBuilder name, out int len)
        {
            int status = nc_inq_dim(ncid, dimid, name, out IntPtr lenp);
            len = (int)lenp;
            return status;
        }
        public static int nc_inq_dimlen(int ncid, int dimid, out int len)
        {
            int status = nc_inq_dimlen(ncid, dimid, out IntPtr lenp);
            len = (int)lenp;
            return status;
        }
        #endregion

        // Wrap all the var1, vara and vars methods so that the calling program can pass the index and count arrays as ints
        #region get_var1
        public static int nc_get_var1_text(int ncid, int varid, int[] index, out byte ip) => nc_get_var1_text(ncid, varid, ConvertToIntPtr(index), out ip);
        public static int nc_get_var1_schar(int ncid, int varid, int[] index, out sbyte ip) => nc_get_var1_schar(ncid, varid, ConvertToIntPtr(index), out ip);
        public static int nc_get_var1_uchar(int ncid, int varid, int[] index, out byte ip) => nc_get_var1_uchar(ncid, varid, ConvertToIntPtr(index), out ip);
        public static int nc_get_var1_short(int ncid, int varid, int[] index, out short ip) => nc_get_var1_short(ncid, varid, ConvertToIntPtr(index), out ip);
        public static int nc_get_var1_int(int ncid, int varid, int[] index, out int ip) => nc_get_var1_int(ncid, varid, ConvertToIntPtr(index), out ip);
        public static int nc_get_var1_long(int ncid, int varid, int[] index, out long ip) => nc_get_var1_long(ncid, varid, ConvertToIntPtr(index), out ip);
        public static int nc_get_var1_float(int ncid, int varid, int[] index, out float ip) => nc_get_var1_float(ncid, varid, ConvertToIntPtr(index), out ip);
        public static int nc_get_var1_double(int ncid, int varid, int[] index, out double ip) => nc_get_var1_double(ncid, varid, ConvertToIntPtr(index), out ip);
        public static int nc_get_var1_ubyte(int ncid, int varid, int[] index, out byte ip) => nc_get_var1_ubyte(ncid, varid, ConvertToIntPtr(index), out ip);
        public static int nc_get_var1_ushort(int ncid, int varid, int[] index, out ushort ip) => nc_get_var1_ushort(ncid, varid, ConvertToIntPtr(index), out ip);
        public static int nc_get_var1_uint(int ncid, int varid, int[] index, out uint ip) => nc_get_var1_uint(ncid, varid, ConvertToIntPtr(index), out ip);
        public static int nc_get_var1_longlong(int ncid, int varid, int[] index, out long ip) => nc_get_var1_longlong(ncid, varid, ConvertToIntPtr(index), out ip);
        public static int nc_get_var1_ulonglong(int ncid, int varid, int[] index, out ulong ip) => nc_get_var1_ulonglong(ncid, varid, ConvertToIntPtr(index), out ip);
        public static int nc_get_var1_string(int ncid, int varid, int[] index, IntPtr[] ip) => nc_get_var1_string(ncid, varid, ConvertToIntPtr(index), ip);
        #endregion

        #region get_vara
        public static int nc_get_vara_text(int ncid, int varid, int[] start, int[] count, byte[] ip) => nc_get_vara_text(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_schar(int ncid, int varid, int[] start, int[] count, sbyte[] ip) => nc_get_vara_schar(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_uchar(int ncid, int varid, int[] start, int[] count, byte[] ip) => nc_get_vara_uchar(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_short(int ncid, int varid, int[] start, int[] count, short[] ip) => nc_get_vara_short(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_int(int ncid, int varid, int[] start, int[] count, int[] ip) => nc_get_vara_int(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_long(int ncid, int varid, int[] start, int[] count, long[] ip) => nc_get_vara_long(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_float(int ncid, int varid, int[] start, int[] count, float[] ip) => nc_get_vara_float(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_double(int ncid, int varid, int[] start, int[] count, double[] ip) => nc_get_vara_double(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_ubyte(int ncid, int varid, int[] start, int[] count, byte[] ip) => nc_get_vara_ubyte(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_ushort(int ncid, int varid, int[] start, int[] count, ushort[] ip) => nc_get_vara_ushort(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_uint(int ncid, int varid, int[] start, int[] count, uint[] ip) => nc_get_vara_uint(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_longlong(int ncid, int varid, int[] start, int[] count, long[] ip) => nc_get_vara_longlong(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_ulonglong(int ncid, int varid, int[] start, int[] count, ulong[] ip) => nc_get_vara_ulonglong(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_string(int ncid, int varid, int[] start, int[] count, IntPtr[] ip) => nc_get_vara_string(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);

        // Multidimensional array support
        public static int nc_get_vara_short(int ncid, int varid, int[] start, int[] count, short[,] ip) => nc_get_vara_short(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_int(int ncid, int varid, int[] start, int[] count, int[,] ip) => nc_get_vara_int(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_long(int ncid, int varid, int[] start, int[] count, long[,] ip) => nc_get_vara_long(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_float(int ncid, int varid, int[] start, int[] count, float[,] ip) => nc_get_vara_float(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_double(int ncid, int varid, int[] start, int[] count, double[,] ip) => nc_get_vara_double(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);

        public static int nc_get_vara_short(int ncid, int varid, int[] start, int[] count, short[,,] ip) => nc_get_vara_short(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_int(int ncid, int varid, int[] start, int[] count, int[,,] ip) => nc_get_vara_int(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_long(int ncid, int varid, int[] start, int[] count, long[,,] ip) => nc_get_vara_long(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_float(int ncid, int varid, int[] start, int[] count, float[,,] ip) => nc_get_vara_float(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);
        public static int nc_get_vara_double(int ncid, int varid, int[] start, int[] count, double[,,] ip) => nc_get_vara_double(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip);

        #endregion

        #region get_vars
        public static int nc_get_vars_text(int ncid, int varid, int[] startp, int[] countp, int[] stridep, byte[] ip) => nc_get_vars_text(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip);
        public static int nc_get_vars_uchar(int ncid, int varid, int[] startp, int[] countp, int[] stridep, byte[] ip) => nc_get_vars_uchar(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip);
        public static int nc_get_vars_schar(int ncid, int varid, int[] startp, int[] countp, int[] stridep, sbyte[] ip) => nc_get_vars_schar(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip);
        public static int nc_get_vars_short(int ncid, int varid, int[] startp, int[] countp, int[] stridep, short[] ip) => nc_get_vars_short(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip);
        public static int nc_get_vars_int(int ncid, int varid, int[] startp, int[] countp, int[] stridep, int[] ip) => nc_get_vars_int(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip);
        public static int nc_get_vars_long(int ncid, int varid, int[] startp, int[] countp, int[] stridep, long[] ip) => nc_get_vars_long(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip);
        public static int nc_get_vars_float(int ncid, int varid, int[] startp, int[] countp, int[] stridep, float[] ip) => nc_get_vars_float(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip);
        public static int nc_get_vars_double(int ncid, int varid, int[] startp, int[] countp, int[] stridep, double[] ip) => nc_get_vars_double(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip);
        public static int nc_get_vars_ushort(int ncid, int varid, int[] startp, int[] countp, int[] stridep, ushort[] ip) => nc_get_vars_ushort(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip);
        public static int nc_get_vars_uint(int ncid, int varid, int[] startp, int[] countp, int[] stridep, uint[] ip) => nc_get_vars_uint(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip);
        public static int nc_get_vars_longlong(int ncid, int varid, int[] startp, int[] countp, int[] stridep, long[] ip) => nc_get_vars_longlong(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip);
        public static int nc_get_vars_ulonglong(int ncid, int varid, int[] startp, int[] countp, int[] stridep, ulong[] ip) => nc_get_vars_ulonglong(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip);
        public static int nc_get_vars_string(int ncid, int varid, int[] startp, int[] countp, int[] stridep, IntPtr[] ip) => nc_get_vars_string(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip);
        #endregion

        #region put_var1
        public static int nc_put_var1_text(int ncid, int varid, int[] index, byte op) => nc_put_var1_text(ncid, varid, ConvertToIntPtr(index), op);
        public static int nc_put_var1_schar(int ncid, int varid, int[] index, sbyte op) => nc_put_var1_schar(ncid, varid, ConvertToIntPtr(index), op);
        public static int nc_put_var1_uchar(int ncid, int varid, int[] index, byte op) => nc_put_var1_uchar(ncid, varid, ConvertToIntPtr(index), op);
        public static int nc_put_var1_short(int ncid, int varid, int[] index, short op) => nc_put_var1_short(ncid, varid, ConvertToIntPtr(index), op);
        public static int nc_put_var1_int(int ncid, int varid, int[] index, int op) => nc_put_var1_int(ncid, varid, ConvertToIntPtr(index), op);
        public static int nc_put_var1_long(int ncid, int varid, int[] index, long op) => nc_put_var1_long(ncid, varid, ConvertToIntPtr(index), op);
        public static int nc_put_var1_float(int ncid, int varid, int[] index, float op) => nc_put_var1_float(ncid, varid, ConvertToIntPtr(index), op);
        public static int nc_put_var1_double(int ncid, int varid, int[] index, double op) => nc_put_var1_double(ncid, varid, ConvertToIntPtr(index), op);
        public static int nc_put_var1_ubyte(int ncid, int varid, int[] index, byte op) => nc_put_var1_ubyte(ncid, varid, ConvertToIntPtr(index), op);
        public static int nc_put_var1_ushort(int ncid, int varid, int[] index, ushort op) => nc_put_var1_ushort(ncid, varid, ConvertToIntPtr(index), op);
        public static int nc_put_var1_uint(int ncid, int varid, int[] index, uint op) => nc_put_var1_uint(ncid, varid, ConvertToIntPtr(index), op);
        public static int nc_put_var1_longlong(int ncid, int varid, int[] index, long op) => nc_put_var1_longlong(ncid, varid, ConvertToIntPtr(index), op);
        public static int nc_put_var1_ulonglong(int ncid, int varid, int[] index, ulong op) => nc_put_var1_ulonglong(ncid, varid, ConvertToIntPtr(index), op);
        public static int nc_put_var1_string(int ncid, int varid, int[] index, string op) => nc_put_var1_string(ncid, varid, ConvertToIntPtr(index), op);
        #endregion

        #region put_vara
        public static int nc_put_vara_text(int ncid, int varid, int[] start, int[] count, byte[] op) => nc_put_vara_text(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_schar(int ncid, int varid, int[] start, int[] count, sbyte[] op) => nc_put_vara_schar(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_uchar(int ncid, int varid, int[] start, int[] count, byte[] op) => nc_put_vara_uchar(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_short(int ncid, int varid, int[] start, int[] count, short[] op) => nc_put_vara_short(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_int(int ncid, int varid, int[] start, int[] count, int[] op) => nc_put_vara_int(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_long(int ncid, int varid, int[] start, int[] count, long[] op) => nc_put_vara_long(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_float(int ncid, int varid, int[] start, int[] count, float[] op) => nc_put_vara_float(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_double(int ncid, int varid, int[] start, int[] count, double[] op) => nc_put_vara_double(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_ubyte(int ncid, int varid, int[] start, int[] count, byte[] op) => nc_put_vara_ubyte(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_ushort(int ncid, int varid, int[] start, int[] count, ushort[] op) => nc_put_vara_ushort(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_uint(int ncid, int varid, int[] start, int[] count, uint[] op) => nc_put_vara_uint(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_longlong(int ncid, int varid, int[] start, int[] count, long[] op) => nc_put_vara_longlong(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_ulonglong(int ncid, int varid, int[] start, int[] count, ulong[] op) => nc_put_vara_ulonglong(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_string(int ncid, int varid, int[] start, int[] count, string[] op) => nc_put_vara_string(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);

        // Multidimensional array support
        public static int nc_put_vara_short(int ncid, int varid, int[] start, int[] count, short[,] op) => nc_put_vara_short(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_int(int ncid, int varid, int[] start, int[] count, int[,] op) => nc_put_vara_int(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_long(int ncid, int varid, int[] start, int[] count, long[,] op) => nc_put_vara_long(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_float(int ncid, int varid, int[] start, int[] count, float[,] op) => nc_put_vara_float(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_double(int ncid, int varid, int[] start, int[] count, double[,] op) => nc_put_vara_double(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);

        public static int nc_put_vara_short(int ncid, int varid, int[] start, int[] count, short[,,] op) => nc_put_vara_short(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_int(int ncid, int varid, int[] start, int[] count, int[,,] op) => nc_put_vara_int(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_long(int ncid, int varid, int[] start, int[] count, long[,,] op) => nc_put_vara_long(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_float(int ncid, int varid, int[] start, int[] count, float[,,] op) => nc_put_vara_float(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        public static int nc_put_vara_double(int ncid, int varid, int[] start, int[] count, double[,,] op) => nc_put_vara_double(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op);
        #endregion

        #region put_vars
        public static int nc_put_vars_text(int ncid, int varid, int[] startp, int[] countp, int[] stridep, byte[] op) => nc_put_vars_text(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op);

        public static int nc_put_vars_uchar(int ncid, int varid, int[] startp, int[] countp, int[] stridep, byte[] op) => nc_put_vars_uchar(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op);

        public static int nc_put_vars_schar(int ncid, int varid, int[] startp, int[] countp, int[] stridep, sbyte[] op) => nc_put_vars_schar(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op);

        public static int nc_put_vars_short(int ncid, int varid, int[] startp, int[] countp, int[] stridep, short[] op) => nc_put_vars_short(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op);

        public static int nc_put_vars_int(int ncid, int varid, int[] startp, int[] countp, int[] stridep, int[] op) => nc_put_vars_int(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op);

        public static int nc_put_vars_long(int ncid, int varid, int[] startp, int[] countp, int[] stridep, long[] op) => nc_put_vars_long(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op);

        public static int nc_put_vars_float(int ncid, int varid, int[] startp, int[] countp, int[] stridep, float[] op) => nc_put_vars_float(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op);

        public static int nc_put_vars_double(int ncid, int varid, int[] startp, int[] countp, int[] stridep, double[] op) => nc_put_vars_double(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op);

        public static int nc_put_vars_ushort(int ncid, int varid, int[] startp, int[] countp, int[] stridep, ushort[] op) => nc_put_vars_ushort(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op);

        public static int nc_put_vars_uint(int ncid, int varid, int[] startp, int[] countp, int[] stridep, uint[] op) => nc_put_vars_uint(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op);

        public static int nc_put_vars_longlong(int ncid, int varid, int[] startp, int[] countp, int[] stridep, long[] op) => nc_put_vars_longlong(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op);

        public static int nc_put_vars_ulonglong(int ncid, int varid, int[] startp, int[] countp, int[] stridep, ulong[] op) => nc_put_vars_ulonglong(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op);

        public static int nc_put_vars_string(int ncid, int varid, int[] startp, int[] countp, int[] stridep, string op) => nc_put_vars_string(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op);
        #endregion

        private static IntPtr[] ConvertToIntPtr(int[] indexp)
        {
            IntPtr[] index = new IntPtr[indexp.Length];
            for (int i = 0; i < index.Length; i++) index[i] = (IntPtr)indexp[i];
            return index;
        }
        #endregion

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
        public static int nc_put_att_text(int ncid, int varid, string name, string value) => nc_put_att_text(ncid, varid, name, value.Length, value);
        public static int nc_put_att_double(int ncid, int varid, string name, double[] value) => nc_put_att_double(ncid, varid, name, nc_type.NC_DOUBLE, value.Length, value);
        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_double(int ncid, int varid, string name, double value)
        {
            double[] v = new double[1];
            v[0] = value;
            return nc_put_att_double(ncid, varid, name, v);
        }
        public static int nc_put_att_float(int ncid, int varid, string name, float[] value) => nc_put_att_float(ncid, varid, name, nc_type.NC_FLOAT, value.Length, value);

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_float(int ncid, int varid, string name, float value)
        {
            float[] v = new float[1];
            v[0] = value;
            return nc_put_att_float(ncid, varid, name, v);
        }
        public static int nc_put_att_int(int ncid, int varid, string name, int[] value) => nc_put_att_int(ncid, varid, name, nc_type.NC_INT, value.Length, value);
        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_int(int ncid, int varid, string name, int value)
        {
            int[] v = new int[1];
            v[0] = value;
            return nc_put_att_int(ncid, varid, name, v);
        }
        public static int nc_put_att_long(int ncid, int varid, string name, long[] value) => nc_put_att_long(ncid, varid, name, nc_type.NC_INT64, value.Length, value);

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_long(int ncid, int varid, string name, long value)
        {
            long[] v = new long[1];
            v[0] = value;
            return nc_put_att_long(ncid, varid, name, v);
        }
        public static int nc_put_att_longlong(int ncid, int varid, string name, long[] value) => nc_put_att_longlong(ncid, varid, name, nc_type.NC_INT64, (IntPtr)value.Length, value);

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_longlong(int ncid, int varid, string name, long value)
        {
            long[] v = new long[1];
            v[0] = value;
            return nc_put_att_long(ncid, varid, name, v);
        }
        public static int nc_put_att_schar(int ncid, int varid, string name, sbyte[] value) => nc_put_att_schar(ncid, varid, name, nc_type.NC_BYTE, (IntPtr)value.Length, value);

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_schar(int ncid, int varid, string name, sbyte value)
        {
            sbyte[] v = new sbyte[1];
            v[0] = value;
            return nc_put_att_schar(ncid, varid, name, v);
        }
        public static int nc_put_att_short(int ncid, int varid, string name, short[] value) => nc_put_att_short(ncid, varid, name, nc_type.NC_SHORT, (IntPtr)value.Length, value);
        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_short(int ncid, int varid, string name, short value)
        {
            short[] v = new short[1];
            v[0] = value;
            return nc_put_att_short(ncid, varid, name, v);
        }

        public static int nc_put_att_ubyte(int ncid, int varid, string name, byte[] value) => nc_put_att_ubyte(ncid, varid, name, nc_type.NC_UBYTE, (IntPtr)value.Length, value);

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_ubyte(int ncid, int varid, string name, byte value)
        {
            byte[] v = new byte[1];
            v[0] = value;
            return nc_put_att_ubyte(ncid, varid, name, v);
        }
        public static int nc_put_att_uchar(int ncid, int varid, string name, byte[] value) => nc_put_att_uchar(ncid, varid, name, nc_type.NC_CHAR, (IntPtr)value.Length, value);
        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_uchar(int ncid, int varid, string name, byte value)
        {
            byte[] v = new byte[1];
            v[0] = value;
            return nc_put_att_uchar(ncid, varid, name, v);
        }
        public static int nc_put_att_uint(int ncid, int varid, string name, uint[] value) => nc_put_att_uint(ncid, varid, name, nc_type.NC_UINT, (IntPtr)value.Length, value);

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_uint(int ncid, int varid, string name, uint value)
        {
            uint[] v = new uint[1];
            v[0] = value;
            return nc_put_att_uint(ncid, varid, name, v);
        }

        public static int nc_put_att_ulonglong(int ncid, int varid, string name, ulong[] value) => nc_put_att_ulonglong(ncid, varid, name, nc_type.NC_UINT64, (IntPtr)value.Length, value);

        /// <summary>Write a single attribute value</summary>
        public static int nc_put_att_ulonglong(int ncid, int varid, string name, ulong value)
        {
            ulong[] v = new ulong[1];
            v[0] = value;
            return nc_put_att_ulonglong(ncid, varid, name, v);
        }

        public static int nc_put_att_ushort(int ncid, int varid, string name, ushort[] value) => nc_put_att_ushort(ncid, varid, name, nc_type.NC_USHORT, (IntPtr)value.Length, value);

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
        # region Methods for strings
        public static string[] nc_get_var_string(int ncid, int varid, out int status)
        {
            int[] dim = new int[1];
            status = nc_inq_vardimid(ncid, varid, dim);
            if (status != 0) return null;
            status = nc_inq_dimlen(ncid, dim[0], out IntPtr length);
            if (status != 0) return null;
            IntPtr[] ptrs = new IntPtr[(int)length];
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
            string[] s = new string[(int)len];
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
            string[] s = new string[(int)len];
            for (int i = 0; i < ptrs.Length; i++) s[i] = Marshal.PtrToStringAnsi(ptrs[i]);
            status = nc_free_string((IntPtr)ptrs.Length, ptrs);
            return s;
        }
        #endregion
        #endregion

        #region Higher level methods
        // These methods wrap up some NetCDF function calls that make them easier to use
        //  but less robust - they will be fine if we know we are using a good NetCDF file or we're happy to skip some error processing
        // Get a global attribute
        public static string GetGlobalAttribute(int ncid, string p_AttName)
        {
            try
            {
                if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out IntPtr len) != 0) return string.Empty;
                if (type == nc_type.NC_INT)
                {
                    int[] att = new int[(int)len];
                    if (nc_get_att_int(ncid, NC_GLOBAL, p_AttName, att) == 0) return att[0].ToString();
                }
                if (type == nc_type.NC_FLOAT)
                {
                    float[] att = new float[(int)len];
                    if (nc_get_att_float(ncid, NC_GLOBAL, p_AttName, att) == 0) return att[0].ToString();
                }
                if (type == nc_type.NC_DOUBLE)
                {
                    double[] att = new double[(int)len];
                    if (nc_get_att_double(ncid, NC_GLOBAL, p_AttName, att) == 0) return att[0].ToString();
                }
                if (type == nc_type.NC_INT64)
                {
                    long[] att = new long[(int)len];
                    if (nc_get_att_longlong(ncid, NC_GLOBAL, p_AttName, att) == 0) return att[0].ToString();
                }

                if (type != nc_type.NC_STRING && type != nc_type.NC_CHAR) return string.Empty;
                StringBuilder sb = new StringBuilder((int)len);
                if (nc_get_att_text(ncid, NC_GLOBAL, p_AttName, sb) != 0) return string.Empty;
                return sb.ToString().Substring(0, (int)len);
            }
            catch (Exception) { return "GetGlobalAttribute for " + p_AttName + " failed"; }
        }        
        
        public static bool GetGlobalAttribute(int ncid, string p_AttName, out int value)
        {
            value = 0;

            if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out IntPtr len) != 0) return false;

            int[] s = new int[(int)len];

            if (nc_get_att_int(ncid, NC_GLOBAL, p_AttName, s) != 0) return false;
            value = s[0];
            return true;
        }

        public static double GetGlobalDouble(int ncid, string p_AttName)
        {
            if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out IntPtr len) != 0) return 0;
            double[] data = new double[(int)len];
            if (nc_get_att_double(ncid, NC_GLOBAL, p_AttName, data) != 0) return 0;
            return data[0];
        }
        public static float GetGlobalFloat(int ncid, string p_AttName)
        {
            if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out IntPtr len) != 0) return 0;
            float[] data = new float[(int)len];
            if (nc_get_att_float(ncid, NC_GLOBAL, p_AttName, data) != 0) return 0;
            return data[0];
        }

        public static short GetGlobalShort(int ncid, string p_AttName)
        {
            if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out IntPtr len) != 0) return 0;
            short[] data = new short[(int)len];
            if (nc_get_att_short(ncid, NC_GLOBAL, p_AttName, data) != 0) return 0;
            return data[0];
        }

        public static int GetGlobalInt(int ncid, string p_AttName)
        {
            if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out IntPtr len) != 0) return 0;
            int[] data = new int[(int)len];
            if (nc_get_att_int(ncid, NC_GLOBAL, p_AttName, data) != 0) return 0;
            return data[0];
        }

        public static bool GetGlobalBool(int ncid, string p_AttName)
        {
            if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out IntPtr len) != 0) return false;
            StringBuilder sb = new StringBuilder((int)len);
            if (nc_get_att_text(ncid, NC_GLOBAL, p_AttName, sb) != 0) return false;
            bool.TryParse(sb.ToString(), out bool result);
            return result;
        }

        // Dates are stored as strings in the metadata - more accessible
        public static DateTime GetGlobalDateTime(int ncid, string p_AttName)
        {
            if (nc_inq_att(ncid, NC_GLOBAL, p_AttName, out nc_type type, out IntPtr len) != 0) return new DateTime();
            StringBuilder sb = new StringBuilder((int)len);
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
            if (nc_inq_att(ncid, varid, p_AttName, out nc_type type, out IntPtr len) != 0) return string.Empty;
            StringBuilder sb = new StringBuilder((int)len);
            if (nc_get_att_text(ncid, varid, p_AttName, sb) != 0) return string.Empty;
            return sb.ToString().Substring(0, (int)len);
        }
        public static bool GetVarAttribute(int ncid, string VarName, string p_AttName, out short value)
        {
            value = 0;

            if (nc_inq_varid(ncid, VarName, out int varid) != 0) return false;
            if (nc_inq_att(ncid, varid, p_AttName, out nc_type type, out IntPtr len) != 0) return false;

            short[] s = new short[(int)len];

            if (nc_get_att_short(ncid, varid, p_AttName, s) != 0) return false;
            value = s[0];
            return true;
        }

        public static bool GetVarAttribute(int ncid, int varid, string p_AttName, out short value)
        {
            value = 0;

            if (nc_inq_att(ncid, varid, p_AttName, out nc_type type, out IntPtr len) != 0) return false;

            short[] s = new short[(int)len];

            if (nc_get_att_short(ncid, varid, p_AttName, s) != 0) return false;
            value = s[0];
            return true;
        }

        public static bool GetVarAttribute(int ncid, string VarName, string p_AttName, out int value)
        {
            value = 0;

            if (nc_inq_varid(ncid, VarName, out int varid) != 0) return false;
            if (nc_inq_att(ncid, varid, p_AttName, out nc_type type, out IntPtr len) != 0) return false;

            int[] s = new int[(int)len];

            if (nc_get_att_int(ncid, varid, p_AttName, s) != 0) return false;
            value = s[0];
            return true;
        }

        public static bool GetVarAttribute(int ncid, string VarName, string p_AttName, out float value)
        {
            value = 0;

            if (nc_inq_varid(ncid, VarName, out int varid) != 0) return false;
            if (nc_inq_att(ncid, varid, p_AttName, out nc_type type, out IntPtr len) != 0) return false;

            float[] s = new float[(int)len];

            if (nc_get_att_float(ncid, varid, p_AttName, s) != 0) return false;
            value = s[0];
            return true;
        }

        public static bool GetVarAttribute(int ncid, int varid, string p_AttName, out float value)
        {
            value = 0;

            if (nc_inq_att(ncid, varid, p_AttName, out nc_type type, out IntPtr len) != 0) return false;

            float[] s = new float[(int)len];

            if (nc_get_att_float(ncid, varid, p_AttName, s) != 0) return false;
            value = s[0];
            return true;
        }

        // Check if a variable exists
        public static bool VarExists(int ncid, string VarName) => nc_inq_varid(ncid, VarName, out int varid) == 0;

        // Get int data
        public static void Get_int(int ncid, string VarName, int[] data)
        {
            nc_inq_varid(ncid, VarName, out int varid);
            nc_get_var_int(ncid, varid, data);
        }

        public static int[] Get_int(int ncid, string VarName)
        {
            nc_inq_varid(ncid, VarName, out int varid);
            nc_inq_dimid(ncid, VarName, out int dimid);
            nc_inq_dimlen(ncid, dimid, out IntPtr PointsCount);
            int[] data = new int[(int)PointsCount];
            nc_get_var_int(ncid, varid, data);
            return data;
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
            nc_inq_dimlen(ncid, dimid, out IntPtr PointsCount);
            float[] data = new float[(int)PointsCount];
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

        public static short[] Get_short(int ncid, string VarName)
        {
            nc_inq_varid(ncid, VarName, out int varid);
            nc_inq_dimid(ncid, VarName, out int dimid);
            nc_inq_dimlen(ncid, dimid, out IntPtr PointsCount);
            short[] data = new short[(int)PointsCount];
            nc_get_var_short(ncid, varid, data);
            return data;
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

        public static byte[] Get_byte(int ncid, string VarName)
        {
            nc_inq_varid(ncid, VarName, out int varid);
            nc_inq_dimid(ncid, VarName, out int dimid);
            nc_inq_dimlen(ncid, dimid, out IntPtr PointsCount);
            byte[] data = new byte[(int)PointsCount];
            nc_get_var_ubyte(ncid, varid, data);
            return data;
        }

        // Methods to write attribute data
        public static void PutGlobalAttribute(int ncid, string AttName, string AttValue) => nc_put_att_text(ncid, NC_GLOBAL, AttName, AttValue.Length, AttValue);
        public static void PutGlobalAttribute(int ncid, string AttName, DateTime AttValue) => PutVarAttribute(ncid, NC_GLOBAL, AttName, AttValue);
        public static void PutGlobalAttribute(int ncid, string AttName, double AttValue) => PutVarAttribute(ncid, NC_GLOBAL, AttName, AttValue);
        public static void PutGlobalAttribute(int ncid, string AttName, float AttValue) => PutVarAttribute(ncid, NC_GLOBAL, AttName, AttValue);
        public static void PutGlobalAttribute(int ncid, string AttName, int AttValue) => PutVarAttribute(ncid, NC_GLOBAL, AttName, AttValue);
        public static void PutGlobalAttribute(int ncid, string AttName, short AttValue) => PutVarAttribute(ncid, NC_GLOBAL, AttName, AttValue);
        public static void PutGlobalAttribute(int ncid, string AttName, bool AttValue) => PutGlobalAttribute(ncid, AttName, AttValue.ToString());
        public static void PutVarAttribute(int ncid, int varid, string AttName, string AttValue) => nc_put_att_text(ncid, varid, AttName, AttValue.Length, AttValue);
        public static void PutVarAttribute(int ncid, int varid, string AttName, DateTime AttValue)
        {
            string date = AttValue.ToString("o");
            nc_put_att_text(ncid, varid, AttName, date.Length, date);
        }
        public static void PutVarAttribute(int ncid, int varid, string AttName, double AttValue)
        {
            double[] att = new double[1];
            att[0] = AttValue;
            nc_put_att_double(ncid, varid, AttName, nc_type.NC_DOUBLE, (IntPtr)att.Length, att);
        }
        public static void PutVarAttribute(int ncid, int varid, string AttName, float AttValue)
        {
            float[] att = new float[1];
            att[0] = AttValue;
            nc_put_att_float(ncid, varid, AttName, nc_type.NC_FLOAT, (IntPtr)att.Length, att);
        }
        public static void PutVarAttribute(int ncid, int varid, string AttName, int AttValue)
        {
            int[] att = new int[1];
            att[0] = AttValue;
            nc_put_att_int(ncid, varid, AttName, nc_type.NC_INT, (IntPtr)att.Length, att);
        }
        public static void PutVarAttribute(int ncid, int varid, string AttName, short AttValue)
        {
            short[] att = new short[1];
            att[0] = AttValue;
            nc_put_att_short(ncid, varid, AttName, nc_type.NC_SHORT, (IntPtr)att.Length, att);
        }
        public static void PutVarAttribute(int ncid, int varid, string AttName, bool AttValue) => PutVarAttribute(ncid, varid, AttName, AttValue.ToString());

        #endregion

        #region Multi-dimensional array support
        // Get methods
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_short(int ncid, int varid, short[,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_int(int ncid, int varid, int[,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_long(int ncid, int varid, long[,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_float(int ncid, int varid, float[,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_double(int ncid, int varid, double[,] ip);


        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_short(int ncid, int varid, short[,,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_int(int ncid, int varid, int[,,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_long(int ncid, int varid, long[,,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_float(int ncid, int varid, float[,,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_double(int ncid, int varid, double[,,] ip);


        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_short(int ncid, int varid, IntPtr[] start, IntPtr[] count, short[,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_int(int ncid, int varid, IntPtr[] start, IntPtr[] count, int[,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_long(int ncid, int varid, IntPtr[] start, IntPtr[] count, long[,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_float(int ncid, int varid, IntPtr[] start, IntPtr[] count, float[,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_double(int ncid, int varid, IntPtr[] start, IntPtr[] count, double[,] ip);


        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_short(int ncid, int varid, IntPtr[] start, IntPtr[] count, short[,,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_int(int ncid, int varid, IntPtr[] start, IntPtr[] count, int[,,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_long(int ncid, int varid, IntPtr[] start, IntPtr[] count, long[,,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_float(int ncid, int varid, IntPtr[] start, IntPtr[] count, float[,,] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_double(int ncid, int varid, IntPtr[] start, IntPtr[] count, double[,,] ip);


        // Put methods
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_short(int ncid, int varid, IntPtr[] start, IntPtr[] count, short[,] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_int(int ncid, int varid, IntPtr[] start, IntPtr[] count, int[,] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_long(int ncid, int varid, IntPtr[] start, IntPtr[] count, long[,] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_float(int ncid, int varid, IntPtr[] start, IntPtr[] count, float[,] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_double(int ncid, int varid, IntPtr[] start, IntPtr[] count, double[,] op);


        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_short(int ncid, int varid, IntPtr[] start, IntPtr[] count, short[,,] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_int(int ncid, int varid, IntPtr[] start, IntPtr[] count, int[,,] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_long(int ncid, int varid, IntPtr[] start, IntPtr[] count, long[,,] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_float(int ncid, int varid, IntPtr[] start, IntPtr[] count, float[,,] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_double(int ncid, int varid, IntPtr[] start, IntPtr[] count, double[,,] op);


        #endregion
    }
}
