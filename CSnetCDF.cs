using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CsNetCDF
{
    public static class netCDF
    {
        #region Constants
        /// <summary>'size' argument to ncdimdef for an unlimited dimension</summary>
        public const int NC_UNLIMITED = 0;
        /// <summary>attribute id to put/get a global attribute</summary>
        public const int NC_GLOBAL = -1;

        /** In HDF5 files you can set the endianness of variables with nc_def_var_endian(). This define is used there. */
        public const int NC_ENDIAN_NATIVE = 0;
        public const int NC_ENDIAN_LITTLE = 1;
        public const int NC_ENDIAN_BIG = 2;

        /** In HDF5 files you can set storage for each variable to be either contiguous or chunked, with nc_def_var_chunking().  This define is
         * used there. */
        public const int NC_CHUNKED = 0;
        public const int NC_CONTIGUOUS = 1;

        /* In HDF5 files you can set check-summing for each variable. Currently the only checksum available is Fletcher-32, which can be set
        with the function nc_def_var_fletcher32.  These defines are used there.
        */
        public const int NC_NOCHECKSUM = 0;
        public const int NC_FLETCHER32 = 1;

        /* Control the HDF5 shuffle filter. In HDF5 files you can specify that a shuffle filter should be used on each chunk of a variable to
         * improve compression for that variable. This per-variable shuffle property can be set with the function nc_def_var_deflate().
         */
        public const int NC_NOSHUFFLE = 0;
        public const int NC_SHUFFLE = 1;

        /// <summary>Minimum deflate level.</summary>
        public const int NC_MIN_DEFLATE_LEVEL = 0;
        /// <summary>Maximum deflate level.</summary>
        public const int NC_MAX_DEFLATE_LEVEL = 9;

        /*  Format specifier for nc_set_default_format() and returned by nc_inq_format. This returns the format as provided by
         *  the API. See nc_inq_format_extended to see the true file format. Starting with version 3.6, there are different format netCDF files.
         *  4.0 introduces the third one. \see netcdf_format
        */
        public const int NC_FORMAT_CLASSIC = 1;
        /* After adding CDF5 support, the NC_FORMAT_64BIT flag is somewhat confusing. So, it is renamed.
           Note that the name in the contributed code NC_FORMAT_64BIT was renamed to NC_FORMAT_CDF2
        */
        public const int NC_FORMAT_64BIT_OFFSET = 2;
        /// <summary>deprecated Saved for compatibility.  Use NC_FORMAT_64BIT_OFFSET or NC_FORMAT_64BIT_DATA, from netCDF 4.4.0 onwards</summary>
        public const int NC_FORMAT_64BIT = (NC_FORMAT_64BIT_OFFSET);
        public const int NC_FORMAT_NETCDF4 = 3;
        public const int NC_FORMAT_NETCDF4_CLASSIC = 4;
        public const int NC_FORMAT_64BIT_DATA = 5;

        /* Alias */
        public const int NC_FORMAT_CDF5 = NC_FORMAT_64BIT_DATA;

        /// <summary>The netcdf external data types</summary>
        public enum nc_type : int
        {
            /// <summary>Not A Type
            /// I've commented this out because it seems a bit pointless</summary>
            //NC_NAT = 0,
            /// <summary>signed 1 byte integer
            /// In C# this is sbyte but the NetCDF variable type is schar (e.g.nc_put_var_schar</summary>
            NC_BYTE = 1,
            /// <summary>ISO/ASCII character</summary>
            NC_CHAR = 2,
            /// <summary>signed 2 byte integer</summary>
            NC_SHORT = 3,
            /// <summary>signed 4 byte integer</summary>
            NC_INT = 4,
            /// <summary>single precision floating point number</summary>
            NC_FLOAT = 5,
            /// <summary>double precision floating point number</summary>
            NC_DOUBLE = 6,
            /// <summary>unsigned 1 byte int 
            /// In C# this is byte but the NetCDF variable type is ubyte (e.g.nc_put_var_ubyte</summary>
            NC_UBYTE = 7,
            /// <summary>unsigned 2-byte int</summary>
            NC_USHORT = 8,
            /// <summary>unsigned 4-byte int </summary>
            NC_UINT = 9,
            /// <summary>signed 8-byte int</summary>
            NC_INT64 = 10,
            /// <summary>unsigned 8-byte int</summary>
            NC_UINT64 = 11,
            /// <summary>string</summary>
            NC_STRING = 12,
        }

        /// <summary>
        ///	Default fill values, used unless _FillValue attribute is set.
        /// These values are stuffed into newly allocated space as appropriate.
        /// The hope is that one might use these to notice that a particular datum
        /// has not been set.
        /// </summary>
        // #define NC_FILL_BYTE    ((signed char)-127)
        // #define NC_FILL_CHAR    ((char)0)
        // #define NC_FILL_SHORT   ((short)-32767)
        // #define NC_FILL_INT     (-2147483647)
        // #define NC_FILL_FLOAT   (9.9692099683868690e+36f) /* near 15 * 2^119 */
        // #define NC_FILL_DOUBLE  (9.9692099683868690e+36)
        // #define NC_FILL_UBYTE   (255)
        // #define NC_FILL_USHORT  (65535)
        // #define NC_FILL_UINT    (4294967295U)
        // #define NC_FILL_INT64   ((long long)-9223372036854775806LL)
        // #define NC_FILL_UINT64  ((unsigned long long)18446744073709551614ULL)
        // #define NC_FILL_STRING  ((char *)"")
        public static class FillValues
        {
            public const sbyte NC_FILL_BYTE = -127;
            public const char NC_FILL_CHAR = (char)0;
            public const short NC_FILL_SHORT = -32767;
            public const int NC_FILL_INT = -2147483647;
            public const float NC_FILL_FLOAT = 9.96921E+36f;    /* near 15 * 2^119 */
            public const double NC_FILL_DOUBLE = 9.969209968386869E+36;
            public const byte NC_FILL_UBYTE = 255;
            public const ushort NC_FILL_USHORT = 65535;
            public const uint NC_FILL_UINT = 4294967295U;
            public const long NC_FILL_INT64 = -9223372036854775806L;
            public const ulong NC_FILL_UINT64 = 18446744073709551614U;
            public const string NC_FILL_STRING = "";
        }

        /// <summary>
        ///	Fill value arrays for use in the corresponding nc_put_att function e.g.
        /// NetCDF.nc_put_att_float(ncid, DataVarid, "_FillValue", NetCDF.NcType.NC_FLOAT, 1, NetCDF.FillVars.FILL_FLOAT);
        /// To save having to define the array each time
        /// </summary>
        public static class FillVars
        {
            public static readonly sbyte[] FILL_BYTE = { FillValues.NC_FILL_BYTE };
            public static readonly char[] FILL_CHAR = { FillValues.NC_FILL_CHAR };
            public static readonly short[] FILL_SHORT = { FillValues.NC_FILL_SHORT };
            public static readonly int[] FILL_INT = { FillValues.NC_FILL_INT };
            public static readonly float[] FILL_FLOAT = { FillValues.NC_FILL_FLOAT };
            public static readonly double[] FILL_DOUBLE = { FillValues.NC_FILL_DOUBLE };
            public static readonly byte[] FILL_UBYTE = { FillValues.NC_FILL_UBYTE };
            public static readonly ushort[] FILL_USHORT = { FillValues.NC_FILL_USHORT };
            public static readonly uint[] FILL_UINT = { FillValues.NC_FILL_UINT };
            public static readonly long[] FILL_INT64 = { FillValues.NC_FILL_INT64 };
            public static readonly ulong[] FILL_UINT64 = { FillValues.NC_FILL_UINT64 };
            public static readonly string[] FILL_STRING = { FillValues.NC_FILL_STRING };
        }

        /*
           * cmode	The creation mode flag. The following flags are available: 
           * NC_CLOBBER (overwrite existing file), 
           * NC_NOCLOBBER (do not overwrite existing file), 
           * NC_SHARE (limit write caching - netcdf classic files only), 
           * NC_64BIT_OFFSET (create 64-bit offset file), 
           * NC_64BIT_DATA (alias NC_CDF5) (create CDF-5 file), 
           * NC_NETCDF4 (create netCDF-4/HDF5 file), 
           * NC_CLASSIC_MODEL (enforce netCDF classic mode on netCDF-4/HDF5 files), 
           * NC_DISKLESS (store data in memory), and NC_PERSIST (force the NC_DISKLESS data from memory to a file), 
           * NC_MMAP (use MMAP for NC_DISKLESS instead of NC_INMEMORY – deprecated). 
           */
        public enum CreateMode : int
        {
            /// <summary>Overwrite existing file. Mode flag for nc_create()</summary>
            NC_CLOBBER = 0x0000,
            /// <summary>Don't destroy existing file. Mode flag for nc_create()</summary>
            NC_NOCLOBBER = 0x0004,
            /// <summary>Use diskless file. Mode flag for nc_open() or nc_create()</summary>
            NC_DISKLESS = 0x0008,
            /// <summary>deprecated Use diskless file with mmap. Mode flag for nc_open() or nc_create()</summary>
            NC_MMAP = 0x0010,
            /// <summary>CDF-5 format: classic model but 64 bit dimensions and sizes</summary>
            NC_64BIT_DATA = 0x0020,
            /// <summary>Enforce classic model on netCDF-4. Mode flag for nc_create()</summary>
            NC_CLASSIC_MODEL = 0x0100,
            /// <summary>Use large (64-bit) file offsets. Mode flag for nc_create()</summary>
            NC_64BIT_OFFSET = 0x0200,
            /// <summary>Share updates, limit caching. Use this in mode flags for both nc_create() and nc_open()</summary>
            NC_SHARE = 0x0800,
            /// <summary>se netCDF-4/HDF5 format. Mode flag for nc_create()</summary>
            NC_NETCDF4 = 0x1000,
            /// <summary>Save diskless contents to disk. Mode flag for nc_open() or nc_create()</summary>
            NC_PERSIST = 0x4000,
        }

        /// <summary>The open mode flags</summary>
        public enum OpenMode : int
        {
            /// <summary>Set read-only access for nc_open()</summary>
            NC_NOWRITE = 0x0000,
            /// <summary>Set read-write access for nc_open()</summary>
            NC_WRITE = 0x0001,
            /// <summary>Share updates, limit caching. Use this in mode flags for both nc_create() and nc_open()</summary>
            NC_SHARE = 0x0800,
            /// <summary>Read from memory. Mode flag for nc_open() or nc_create()</summary>
            NC_INMEMORY = 0x8000
        }  
        #endregion

        #region CustomMarshaller
        // From https://stackoverflow.com/questions/6300093/why-cant-i-return-a-char-string-from-c-to-c-sharp-in-a-release-build
        class ConstCharPtrMarshaler : ICustomMarshaler
        {
            public object MarshalNativeToManaged(IntPtr pNativeData) => Marshal.PtrToStringAnsi(pNativeData);
            public IntPtr MarshalManagedToNative(object ManagedObj) => IntPtr.Zero;
            public void CleanUpNativeData(IntPtr pNativeData) {}
            public void CleanUpManagedData(object ManagedObj) {}
            public int GetNativeDataSize() => IntPtr.Size;
            static readonly ConstCharPtrMarshaler instance = new ConstCharPtrMarshaler();
            public static ICustomMarshaler GetInstance(string cookie) => instance;
        }
        #endregion

        #region Methods
        //
        // Methods returning const char * require the custom Marshaller
        //
        /// <summary>Return the library version string</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string nc_inq_libvers();

        /// <summary>Return the error message</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string nc_strerror(int ncerr);

        #region C# Friendly methods
        //
        // C# Friendly methods for returning strings
        //
        /// <summary>Friendly method for nc_inq_path</summary>
        public static string nc_inq_path(int ncid)
        {
            nc_inq_path(ncid, out int len, null);
            StringBuilder sb = new StringBuilder(len);
            nc_inq_path(ncid, out len, sb);
            return sb.ToString();
        }

        /// <summary>Friendly method for nc_inq_grpname_full</summary>
        public static string nc_inq_grpname_full(int ncid)
        {
            nc_inq_grpname_full(ncid, out int len,null);
            StringBuilder sb = new StringBuilder(len);
            nc_inq_grpname_full(ncid, out len, sb);
            return sb.ToString();
        }

        //
        // Friendly methods for reading attributes
        //
        public static string nc_get_att_text(int ncid, int varid, string name)
        {
            nc_inq_att(ncid, varid, name, out nc_type nctype, out int length);
            StringBuilder sb = new StringBuilder(length);
            nc_get_att_text(ncid, varid, name, sb);
            return sb.ToString();
        }
        public static sbyte[] nc_get_att_schar(int ncid, int varid, string name)
        {
            nc_inq_att(ncid, varid, name, out nc_type nctype, out int length);
            sbyte[] value = new sbyte[length];
            nc_get_att_schar(ncid, varid, name, value);
            return value;
        }
        public static byte[] nc_get_att_uchar(int ncid, int varid, string name)
        {
            nc_inq_att(ncid, varid, name, out nc_type nctype, out int length);
            byte[] value = new byte[length];
            nc_get_att_uchar(ncid, varid, name, value);
            return value;
        }
        public static short[] nc_get_att_short(int ncid, int varid, string name)
        {
            nc_inq_att(ncid, varid, name, out nc_type nctype, out int length);
            short[] value = new short[length];
            nc_get_att_short(ncid, varid, name, value);
            return value;
        }
        public static int[] nc_get_att_int(int ncid, int varid, string name)
        {
            nc_inq_att(ncid, varid, name, out nc_type nctype, out int length);
            int[] value = new int[length];
            nc_get_att_int(ncid, varid, name, value);
            return value;
        }
        public static long[] nc_get_att_long(int ncid, int varid, string name)
        {
            nc_inq_att(ncid, varid, name, out nc_type nctype, out int length);
            long[] value = new long[length];
            nc_get_att_long(ncid, varid, name, value);
            return value;
        }
        public static float[] nc_get_att_float(int ncid, int varid, string name)
        {
            nc_inq_att(ncid, varid, name, out nc_type nctype, out int length);
            float[] value = new float[length];
            nc_get_att_float(ncid, varid, name, value);
            return value;
        }
        public static double[] nc_get_att_double(int ncid, int varid, string name)
        {
            nc_inq_att(ncid, varid, name, out nc_type nctype, out int length);
            double[] value = new double[length];
            nc_get_att_double(ncid, varid, name, value);
            return value;
        }
        public static byte[] nc_get_att_ubyte(int ncid, int varid, string name)
        {
            nc_inq_att(ncid, varid, name, out nc_type nctype, out int length);
            byte[] value = new byte[length];
            nc_get_att_ubyte(ncid, varid, name, value);
            return value;
        }
        public static ushort[] nc_get_att_ushort(int ncid, int varid, string name)
        {
            nc_inq_att(ncid, varid, name, out nc_type nctype, out int length);
            ushort[] value = new ushort[length];
            nc_get_att_ushort(ncid, varid, name, value);
            return value;
        }
        public static uint[] nc_get_att_uint(int ncid, int varid, string name)
        {
            nc_inq_att(ncid, varid, name, out nc_type nctype, out int length);
            uint[] value = new uint[length];
            nc_get_att_uint(ncid, varid, name, value);
            return value;
        }
        public static long[] nc_get_att_longlong(int ncid, int varid, string name)
        {
            nc_inq_att(ncid, varid, name, out nc_type nctype, out int length);
            long[] value = new long[length];
            nc_get_att_longlong(ncid, varid, name, value);
            return value;
        }
        public static ulong[] nc_get_att_ulonglong(int ncid, int varid, string name)
        {
            nc_inq_att(ncid, varid, name, out nc_type nctype, out int length);
            ulong[] value = new ulong[length];
            nc_get_att_ulonglong(ncid, varid, name, value);
            return value;
        }

        //
        //  Friendly methods for dealing with strings and other awkward data types
        //
        // Methods for strings
        public static string[] nc_get_var_string(int ncid, int varid)
        {
            int[] dim = new int[1];
            int status = nc_inq_vardimid(ncid, varid, dim);
            status = nc_inq_dimlen(ncid, dim[0], out int length);
            IntPtr[] ptrs = new IntPtr[length];
            status = nc_get_var_string(ncid, varid, ptrs);
            string[] s = new string[ptrs.Length];
            for (int i = 0; i < ptrs.Length; i++) s[i] = Marshal.PtrToStringAnsi(ptrs[i]);
            status = nc_free_string((IntPtr)ptrs.Length, ptrs);
            return s;
        }

        public static string nc_get_var1_string(int ncid, int varid, int[] index)
        {
            IntPtr[] ptrs = new IntPtr[1];
            int status = nc_get_var1_string(ncid, varid, index, ptrs);
            string s;
            s = Marshal.PtrToStringAnsi(ptrs[0]);
            status = nc_free_string((IntPtr)ptrs.Length, ptrs);
            return s;
        }

        public static string[] nc_get_vara_string(int ncid, int varid, int[] start, int[] count)
        {
            int len = 0;
            for (int i = 0; i < count.Length; i++) len += count[i];
            IntPtr[] ptrs = new IntPtr[len];
            int status = nc_get_vara_string(ncid, varid, start, count, ptrs);
            string[] s = new string[len];
            for (int i = 0; i < ptrs.Length; i++) s[i] = Marshal.PtrToStringAnsi(ptrs[i]);
            status = nc_free_string((IntPtr)ptrs.Length, ptrs);
            return s;
        }

        public static string[] nc_get_vars_string(int ncid, int varid, int[] start, int[] count, int[] stride)
        {
            int len = 0;
            for (int i = 0; i < count.Length; i++) len += count[i]/stride[i];
            IntPtr[] ptrs = new IntPtr[len];
            int status = nc_get_vars_string(ncid, varid, start, count, stride, ptrs);
            string[] s = new string[len];
            for (int i = 0; i < ptrs.Length; i++) s[i] = Marshal.PtrToStringAnsi(ptrs[i]);
            status = nc_free_string((IntPtr)ptrs.Length, ptrs);
            return s;
        }

        #endregion

        #region IO methods
        //
        // IO
        //
        /// <summary>Close an open netCDF dataset</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_close(int ncid);

        /// <summary>Create a new netCDF file.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_create(string path, CreateMode mode, out int ncidp);

        /// <summary>Create a netCDF file with the contents stored in memory.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_create_mem(string path, CreateMode mode, int initialsize, out int ncidp);

        /// <summary>Leave define mode</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_enddef(int ncidp);

        /// <summary>Inquire about a file or group.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq(int ncid, out int ndims, out int nvars, out int ngatts, out int unlimdimid);

        /// <summary>Inquire about the binary format of a netCDF file as presented by the API.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_format(int ncid, out int format);

        /// <summary>Obtain more detailed (vis-a-vis nc_inq_format) format information about an open dataset.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_format_extended(int ncid, out int format, out int mode);

        /// <summary>Learn the path used to open/create the file. 
        /// Use nc_inq_path(ncid) instead, otherwise a correctly sized StringBuilder is required</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_path(int ncid, out int pathlen, StringBuilder path);

        /// <summary>Open an existing netCDF file.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_open(string path, OpenMode mode, out int ncidp);

        /// <summary>Put open netcdf dataset into define mode</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_redef(int ncid);

        /// <summary>Set the fill mode (classic or 64-bit offset files only).</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_set_fill(int ncid, int fillmode, out int old_modep);

        /// <summary>Synchronize an open netcdf dataset to disk</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_sync(int ncid);
        #endregion

        #region Dimensions
        //
        // Dimensions
        //
        /// <summary>Define a new dimension.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_dim(int ncid, string name, int len, out int dimidp);

        /// <summary>Find the name and length of a dimension.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_dim(int ncid, int dimid, string name, out int length);

        /// <summary>Find the ID of a dimension from the name.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_dimid(int ncid, string name, out int dimid);

        /// <summary>Find the length of a dimension.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_dimlen(int ncid, int dimid, out int length);

        /// <summary>Find out the name of a dimension.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_dimname(int ncid, int dimid, StringBuilder name);

        /// <summary>Find the number of dimensions.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_ndims(int ncid, out int ndims);

        /// <summary>Find the ID of the unlimited dimension.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_unlimdim(int ncid, out int unlimdimid);

        /// <summary>Rename a dimension.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_rename_dim(int ncid, int dimid, string name);
        #endregion

        #region Variables - Defining and learning
        //
        // Defining Variables
        // Learning about Variables
        //
        /// <summary>Set the per-variable cache size, nelems, and preemption policy. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_set_var_chunk_cache(int ncid, int varid, int size, int nelems, float preemption);

        /// <summary>Get the per-variable cache size, nelems, and preemption policy.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_chunk_cache(int ncid, int varid, out int sizep, out int nelemsp, out float preemptionp);

        /// <summary>Define a variable</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_var(int ncid, string name, nc_type xtype, int ndims, int[] dimids, out int varidp);

        /// <summary>Set compression settings for a variable. Lower is faster, higher is better.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_var_deflate(int ncid, int varid, int shuffle, int deflate, int deflate_level);

        /// <summary>Find out compression settings of a var.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_var_deflate(int ncid, int varid, out int shufflep, out int deflatep, out int deflate_levelp);

        /// <summary>Find out szip settings of a var.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_var_szip(int ncid, int varid, out int options_maskp, out int pixels_per_blockp);

        /// <summary>Set fletcher32 checksum for a var. This must be done after nc_def_var</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_var_fletcher32(int ncid, int varid, int fletcher32);

        /// <summary>Inquire about fletcher32 checksum for a var.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_var_fletcher32(int ncid, int varid, out int fletcher32p);

        /// <summary>Define chunking for a variable. This must be done after nc_def_var</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_var_chunking(int ncid, int varid, int storage, out int chunksizesp);

        /// <summary>Inq chunking stuff for a var.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_var_chunking(int ncid, int varid, out int storagep, out int chunksizesp);

        /// <summary>Define fill value behavior for a variable. This must be done after nc_def_var</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_var_fill(int ncid, int varid, int no_fill, int fill_value);

        /// <summary>Inq fill value setting for a var.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_var_fill(int ncid, int varid, out int no_fill, out int fill_valuep);

        /// <summary>Define the endianness of a variable.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_var_endian(int ncid, int varid, int endian);

        /// <summary>Learn about the endianness of a variable.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_var_endian(int ncid, int varid, out int endianp);

        /// <summary>Define a filter for a variable</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_var_filter(int ncid, int varid, uint id, int nparams, out uint parms);

        /// <summary>Learn about the filter on a variable</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_var_filter(int ncid, int varid, out uint idp, out int nparams, out uint parms);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_var(int ncid, int varid, string name, out nc_type type, out int ndims, int[] dimids, out int natts);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_varids(int ncid, out int nvars, int[] varids);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_vartype(int ncid, int varid, out nc_type xtypep);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_varnatts(int ncid, int varid, out int nattsp);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_varid(int ncid, string name, out int varidp);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_nvars(int ncid, out int nvars);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_varname(int ncid, int varid, StringBuilder name);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_varndims(int ncid, int varid, out int ndims);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_vardimid(int ncid, int varid, int[] dimids);

        #endregion

        #region Attributes
        //
        //  Attributes
        //      
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_natts(int ncid, out int ngatts);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_attname(int ncid, int varid, int attnum, StringBuilder name);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_att(int ncid, int varid, string name, out nc_type type, out int length);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_att_text(int ncid, int varid, string name, StringBuilder value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_att_schar(int ncid, int varid, string name, sbyte[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_att_uchar(int ncid, int varid, string name, byte[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_att_short(int ncid, int varid, string name, short[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_att_int(int ncid, int varid, string name, int[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_att_long(int ncid, int varid, string name, long[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_att_float(int ncid, int varid, string name, float[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_att_double(int ncid, int varid, string name, double[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_att_ubyte(int ncid, int varid, string name, byte[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_att_ushort(int ncid, int varid, string name, ushort[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_att_uint(int ncid, int varid, string name, uint[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_att_longlong(int ncid, int varid, string name, long[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_att_ulonglong(int ncid, int varid, string name, ulong[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_text(int ncid, int varid, string name, int len, string value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_schar(int ncid, int varid, string name, nc_type type, int len, sbyte[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_uchar(int ncid, int varid, string name, nc_type type, int len, byte[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_short(int ncid, int varid, string name, nc_type type, int len, short[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_int(int ncid, int varid, string name, nc_type type, int len, int[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_long(int ncid, int varid, string name, nc_type type, int len, long[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_float(int ncid, int varid, string name, nc_type type, int len, float[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_double(int ncid, int varid, string name, nc_type type, int len, double[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_ubyte(int ncid, int varid, string name, nc_type type, int len, byte[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_ushort(int ncid, int varid, string name, nc_type type, int len, ushort[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_uint(int ncid, int varid, string name, nc_type type, int len, uint[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_longlong(int ncid, int varid, string name, nc_type type, int len, long[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_ulonglong(int ncid, int varid, string name, nc_type type, int len, ulong[] value);
        #endregion

        #region Variables - Reading
        //
        // Reading values from variables
        //
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_text(int ncid, int varid, byte[] ip);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_schar(int ncid, int varid, sbyte[] ip);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_uchar(int ncid, int varid, byte[] ip);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_short(int ncid, int varid, short[] ip);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_int(int ncid, int varid, int[] ip);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_long(int ncid, int varid, long[] ip);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_float(int ncid, int varid, float[] ip);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_double(int ncid, int varid, double[] ip);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_ubyte(int ncid, int varid, byte[] ip);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_ushort(int ncid, int varid, ushort[] ip);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_uint(int ncid, int varid, uint[] ip);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_longlong(int ncid, int varid, long[] ip);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_ulonglong(int ncid, int varid, ulong[] ip);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_string(int ncid, int varid, IntPtr[] ip);

        //
        // x86 versions of get_var1
        //
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_text(int ncid, int varid, int[] index, out byte ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_schar(int ncid, int varid, int[] index, out sbyte ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_uchar(int ncid, int varid, int[] index, out byte ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_short(int ncid, int varid, int[] index, out short ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_int(int ncid, int varid, int[] index, out int ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_long(int ncid, int varid, int[] index, out long ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_float(int ncid, int varid, int[] index, out float ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_double(int ncid, int varid, int[] index, out double ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_ubyte(int ncid, int varid, int[] index, out byte ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_ushort(int ncid, int varid, int[] index, out ushort ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_uint(int ncid, int varid, int[] index, out uint ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_longlong(int ncid, int varid, int[] index, out long ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_ulonglong(int ncid, int varid, int[] index, out ulong ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_string(int ncid, int varid, int[] index, IntPtr[] ip);

        //
        // x64 versions of get_var1
        //
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_text(int ncid, int varid, long[] index, out byte ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_schar(int ncid, int varid, long[] index, out sbyte ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_uchar(int ncid, int varid, long[] index, out byte ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_short(int ncid, int varid, long[] index, out short ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_int(int ncid, int varid, long[] index, out int ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_long(int ncid, int varid, long[] index, out long ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_float(int ncid, int varid, long[] index, out float ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_double(int ncid, int varid, long[] index, out double ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_ubyte(int ncid, int varid, long[] index, out byte ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_ushort(int ncid, int varid, long[] index, out ushort ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_uint(int ncid, int varid, long[] index, out uint ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_longlong(int ncid, int varid, long[] index, out long ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_ulonglong(int ncid, int varid, long[] index, out ulong ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_string(int ncid, int varid, long[] index, out StringBuilder ip);

        //
        // x86 versions of get_vara
        //
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_text(int ncid, int varid, int[] start, int[] count, byte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_schar(int ncid, int varid, int[] start, int[] count, sbyte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_uchar(int ncid, int varid, int[] start, int[] count, byte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_short(int ncid, int varid, int[] start, int[] count, short[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_int(int ncid, int varid, int[] start, int[] count, int[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_long(int ncid, int varid, int[] start, int[] count, long[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_float(int ncid, int varid, int[] start, int[] count, float[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_double(int ncid, int varid, int[] start, int[] count, double[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_ubyte(int ncid, int varid, int[] start, int[] count, byte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_ushort(int ncid, int varid, int[] start, int[] count, ushort[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_uint(int ncid, int varid, int[] start, int[] count, uint[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_longlong(int ncid, int varid, int[] start, int[] count, long[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_ulonglong(int ncid, int varid, int[] start, int[] count, ulong[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_string(int ncid, int varid, int[] start, int[] count, IntPtr[] ip);

        //
        // x64 versions of get_vara
        //
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_text(int ncid, int varid, long[] start, long[] count, byte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_schar(int ncid, int varid, long[] start, long[] count, sbyte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_uchar(int ncid, int varid, long[] start, long[] count, byte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_short(int ncid, int varid, long[] start, long[] count, short[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_int(int ncid, int varid, long[] start, long[] count, int[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_long(int ncid, int varid, long[] start, long[] count, long[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_float(int ncid, int varid, long[] start, long[] count, float[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_double(int ncid, int varid, long[] start, long[] count, double[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_ubyte(int ncid, int varid, long[] start, long[] count, byte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_ushort(int ncid, int varid, long[] start, long[] count, ushort[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_uint(int ncid, int varid, long[] start, long[] count, uint[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_longlong(int ncid, int varid, long[] start, long[] count, long[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_ulonglong(int ncid, int varid, long[] start, long[] count, ulong[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_string(int ncid, int varid, long[] start, long[] count, string[] ip);
        #endregion

        #region Variables - writing
        //
        // x86 versions of get_vars/put_vars
        //
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_text(int ncid, int varid, int[] startp, int[] countp, int[] stridep, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_text(int ncid, int varid, int[] startp, int[] countp, int[] stridep, byte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_uchar(int ncid, int varid, int[] startp, int[] countp, int[] stridep, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_uchar(int ncid, int varid, int[] startp, int[] countp, int[] stridep, byte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_schar(int ncid, int varid, int[] startp, int[] countp, int[] stridep, sbyte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_schar(int ncid, int varid, int[] startp, int[] countp, int[] stridep, sbyte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_short(int ncid, int varid, int[] startp, int[] countp, int[] stridep, short[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_short(int ncid, int varid, int[] startp, int[] countp, int[] stridep, short[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_int(int ncid, int varid, int[] startp, int[] countp, int[] stridep, int[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_int(int ncid, int varid, int[] startp, int[] countp, int[] stridep, out int[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_long(int ncid, int varid, int[] startp, int[] countp, int[] stridep, long[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_long(int ncid, int varid, int[] startp, int[] countp, int[] stridep, out long[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_float(int ncid, int varid, int[] startp, int[] countp, int[] stridep, float[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_float(int ncid, int varid, int[] startp, int[] countp, int[] stridep, out float[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_double(int ncid, int varid, int[] startp, int[] countp, int[] stridep, double[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_double(int ncid, int varid, int[] startp, int[] countp, int[] stridep, out double[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_ushort(int ncid, int varid, int[] startp, int[] countp, int[] stridep, ushort[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_ushort(int ncid, int varid, int[] startp, int[] countp, int[] stridep, out ushort[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_uint(int ncid, int varid, int[] startp, int[] countp, int[] stridep, uint[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_uint(int ncid, int varid, int[] startp, int[] countp, int[] stridep, uint[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_longlong(int ncid, int varid, int[] startp, int[] countp, int[] stridep, long[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_longlong(int ncid, int varid, int[] startp, int[] countp, int[] stridep, long[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_ulonglong(int ncid, int varid, int[] startp, int[] countp, int[] stridep, ulong[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_ulonglong(int ncid, int varid, int[] startp, int[] countp, int[] stridep, ulong[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_string(int ncid, int varid, int[] startp, int[] countp, int[] stridep, string op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_string(int ncid, int varid, int[] startp, int[] countp, int[] stridep, IntPtr[] ip);
        #endregion

        //
        // x64 versions of get_vars/put_vars
        //
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_text(int ncid, int varid, long[] startp, long[] countp, long[] stridep, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_text(int ncid, int varid, long[] startp, long[] countp, long[] stridep, byte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_uchar(int ncid, int varid, long[] startp, long[] countp, long[] stridep, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_uchar(int ncid, int varid, long[] startp, long[] countp, long[] stridep, byte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_schar(int ncid, int varid, long[] startp, long[] countp, long[] stridep, sbyte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_schar(int ncid, int varid, long[] startp, long[] countp, long[] stridep, sbyte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_short(int ncid, int varid, long[] startp, long[] countp, long[] stridep, short[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_short(int ncid, int varid, long[] startp, long[] countp, long[] stridep, short[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_int(int ncid, int varid, long[] startp, long[] countp, long[] stridep, int[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_int(int ncid, int varid, long[] startp, long[] countp, long[] stridep, out int[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_long(int ncid, int varid, long[] startp, long[] countp, long[] stridep, long[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_long(int ncid, int varid, long[] startp, long[] countp, long[] stridep, out long[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_float(int ncid, int varid, long[] startp, long[] countp, long[] stridep, float[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_float(int ncid, int varid, long[] startp, long[] countp, long[] stridep, out float[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_double(int ncid, int varid, long[] startp, long[] countp, long[] stridep, double[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_double(int ncid, int varid, long[] startp, long[] countp, long[] stridep, out double[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_ushort(int ncid, int varid, long[] startp, long[] countp, long[] stridep, ushort[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_ushort(int ncid, int varid, long[] startp, long[] countp, long[] stridep, out ushort[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_uint(int ncid, int varid, long[] startp, long[] countp, long[] stridep, uint[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_uint(int ncid, int varid, long[] startp, long[] countp, long[] stridep, uint[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_longlong(int ncid, int varid, long[] startp, long[] countp, long[] stridep, long[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_longlong(int ncid, int varid, long[] startp, long[] countp, long[] stridep, long[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_ulonglong(int ncid, int varid, long[] startp, long[] countp, long[] stridep, ulong[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_ulonglong(int ncid, int varid, long[] startp, long[] countp, long[] stridep, ulong[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_string(int ncid, int varid, long[] startp, long[] countp, long[] stridep, string op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_string(int ncid, int varid, long[] startp, long[] countp, long[] stridep, string[] ip);

        //
        // Write variables
        //
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var_text(int ncid, int varid, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var_schar(int ncid, int varid, sbyte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var_uchar(int ncid, int varid, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var_short(int ncid, int varid, short[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var_int(int ncid, int varid, int[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var_long(int ncid, int varid, long[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var_float(int ncid, int varid, float[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var_double(int ncid, int varid, double[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var_ubyte(int ncid, int varid, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var_ushort(int ncid, int varid, ushort[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var_uint(int ncid, int varid, uint[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var_longlong(int ncid, int varid, long[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var_ulonglong(int ncid, int varid, ulong[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var_string(int ncid, int varid, string[] op);

        //
        // x86 versions of put_var1
        //
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_text(int ncid, int varid, int[] index, byte op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_schar(int ncid, int varid, int[] index, sbyte op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_uchar(int ncid, int varid, int[] index, byte op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_short(int ncid, int varid, int[] index, short op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_int(int ncid, int varid, int[] index, int op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_long(int ncid, int varid, int[] index, long op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_float(int ncid, int varid, int[] index, float op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_double(int ncid, int varid, int[] index, double op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_ubyte(int ncid, int varid, int[] index, byte op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_ushort(int ncid, int varid, int[] index, ushort op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_uint(int ncid, int varid, int[] index, uint op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_longlong(int ncid, int varid, int[] index, long op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_ulonglong(int ncid, int varid, int[] index, ulong op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_string(int ncid, int varid, int[] index, string op);

        //
        // x64 versions
        //
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_text(int ncid, int varid, long[] index, byte op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_schar(int ncid, int varid, long[] index, sbyte op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_uchar(int ncid, int varid, long[] index, byte op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_short(int ncid, int varid, long[] index, short op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_int(int ncid, int varid, long[] index, int op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_long(int ncid, int varid, long[] index, long op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_float(int ncid, int varid, long[] index, float op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_double(int ncid, int varid, long[] index, double op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_ubyte(int ncid, int varid, long[] index, byte op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_ushort(int ncid, int varid, long[] index, ushort op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_uint(int ncid, int varid, long[] index, uint op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_longlong(int ncid, int varid, long[] index, long op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_ulonglong(int ncid, int varid, long[] index, ulong op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_string(int ncid, int varid, long[] index, string op);


        //
        // x86 versions of put_vara
        //
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_text(int ncid, int varid, int[] start, int[] count, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_schar(int ncid, int varid, int[] start, int[] count, sbyte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_uchar(int ncid, int varid, int[] start, int[] count, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_short(int ncid, int varid, int[] start, int[] count, short[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_int(int ncid, int varid, int[] start, int[] count, int[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_long(int ncid, int varid, int[] start, int[] count, long[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_float(int ncid, int varid, int[] start, int[] count, float[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_double(int ncid, int varid, int[] start, int[] count, double[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_ubyte(int ncid, int varid, int[] start, int[] count, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_ushort(int ncid, int varid, int[] start, int[] count, ushort[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_uint(int ncid, int varid, int[] start, int[] count, uint[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_longlong(int ncid, int varid, int[] start, int[] count, long[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_ulonglong(int ncid, int varid, int[] start, int[] count, ulong[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_string(int ncid, int varid, int[] start, int[] count, string[] op);

        //
        // x64 versions of put_vara
        //
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_text(int ncid, int varid, long[] start, long[] count, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_schar(int ncid, int varid, long[] start, long[] count, sbyte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_uchar(int ncid, int varid, long[] start, long[] count, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_short(int ncid, int varid, long[] start, long[] count, short[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_int(int ncid, int varid, long[] start, long[] count, int[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_long(int ncid, int varid, long[] start, long[] count, long[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_float(int ncid, int varid, long[] start, long[] count, float[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_double(int ncid, int varid, long[] start, long[] count, double[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_ubyte(int ncid, int varid, long[] start, long[] count, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_ushort(int ncid, int varid, long[] start, long[] count, ushort[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_uint(int ncid, int varid, long[] start, long[] count, uint[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_longlong(int ncid, int varid, long[] start, long[] count, long[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_ulonglong(int ncid, int varid, long[] start, long[] count, ulong[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_string(int ncid, int varid, long[] start, long[] count, string[] op);


        //
        // Group methods
        //
        /// <summary>Given an ncid and group name (NULL gets root group), return locid. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_ncid(int ncid, string name, out int grp_ncid);

        /// <summary>Given a location id, return the number of groups it contains, and an array of their locids.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_grps(int ncid, out int numgrps, out int ncids);

        /// <summary>Given locid, find name of group. (Root group is named "/".) </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_grpname(int ncid, StringBuilder name);

        /// <summary>
        /// Given ncid, find full name and len of full name. (Root group is named "/", with length 1.) 
        /// But use nc_inq_grpname_full(ncid) instead
        /// </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_grpname_full(int ncid, out int lenp, StringBuilder full_name);

        /// <summary>Given ncid, find len of full name. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_grpname_len(int ncid, out int lenp);

        /// <summary>Given an ncid, find the ncid of its parent group.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_grp_parent(int ncid, out int parent_ncid);

        /// <summary>Given a name and parent ncid, find group ncid.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_grp_ncid(int ncid, string grp_name, out int grp_ncid);

        /// <summary>Given a full name and ncid, find group ncid.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_grp_full_ncid(int ncid, string full_name, out int grp_ncid);

        // Enum types
        /// <summary>
        /// Create an enum type. Provide a base type and a name. At the moment
        /// only ints are accepted as base types. 
        /// </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_enum(int ncid, nc_type base_typeid, string name, out nc_type typeidp);

        /// <summary>Insert a named value into an enum type.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_insert_enum(int ncid, nc_type xtype, string name, object value);

        /// <summary>Get information about an enum type: its name, base type and the number of members defined. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_enum(int ncid, nc_type xtype, StringBuilder name, out nc_type base_nc_typep, out int base_sizep, out int num_membersp);

        /// <summary>Get information about an enum member</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_enum_member(int ncid, nc_type xtype, int idx, string name, out object value);

        /// <summary>Get enum name from enum value. Name size will be <= NC_MAX_NAME.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_enum_ident(int ncid, nc_type xtype, long value, StringBuilder identifier);

        /// <summary>
        /// Set the default nc_create format to NC_FORMAT_CLASSIC, NC_FORMAT_64BIT,
        /// NC_FORMAT_CDF5, NC_FORMAT_NETCDF4, or NC_FORMAT_NETCDF4_CLASSIC 
        /// </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_set_default_format(int format, out int old_formatp);

        /// <summary>Set the cache size, nelems, and preemption policy.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_set_chunk_cache(int size, int nelems, float preemption);

        /// <summary>Get the cache size, nelems, and preemption policy.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_chunk_cache(out int sizep, out int nelemsp, out float preemptionp);
        #endregion

        #region Misc methods
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_free_string(IntPtr len, IntPtr[] data);

        // Multi dimensional array support
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var_float(int ncid, int varid, float[,] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_float(int ncid, int varid, float[,] ip);
        #endregion
    }
}
