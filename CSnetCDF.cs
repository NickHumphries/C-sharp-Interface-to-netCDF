/*
 * A C# interface to the UniData NetCDF dll.
 * Nick Humphries   April 2021
 *     
 * This C# interface supports the functions provided by the Unidata netcdf.dll (https://www.unidata.ucar.edu/software/netcdf/) 
 * currently up to 4.8.0 (2021-03-31), although not all functions are supported (e.g. I have omitted the deprecated *varm* functions)
 * 
 * netCDF: doi:10.5065/D6H70CW6 https://doi.org/10.5065/D6H70CW6
 * 
 * This file supports both x86 and x64 versions of the dlls, by defining the index[] start[] and count[] arrays for get_vara, get_var1 and get_vars methods as IntPtr.
 * Wrappers for these methods have been provided so that these arrays can always be defined in the calling program as int[].
 * Note that this also applies to lengths, such as returned from nc_inq_dimlen, where again the lengths are defined as IntPtr but wrappers exists to allow int to be used.
 * 
 * A collection of C# friendly methods have been provided to simplify calls to functions returning string variables and also for 
 * getting and putting attributes. Thanks to https://stackoverflow.com/questions/6300093/why-cant-i-return-a-char-string-from-c-to-c-sharp-in-a-release-build 
 * (https://stackoverflow.com/users/1164966/benoit-blanchon) for the custom marshaller.
 *
 * Also, I have provided a couple of examples of how to get or put multidimensional arrays without reformatting. 
 * Any of the functions can be copied and modified to provide direct access to multidimensional arrays simply by changing the method from: 
 * 
 *          nc_put_var_float(int ncid, int varid, float[] op);
 * 
 *          to nc_put_var_float(int ncid, int varid, float[,] op);
 *          or nc_put_var_float(int ncid, int varid, float[,,] op);
 *                
 * Data types
 * Some of the data types supported by the netCDF dll do not map exactly to C# data types
 * The following netCDF data types are defined:
 *             
 *      NC_BYTE     C# sbyte
 *      NC_CHAR     C# byte
 *      NC_SHORT    C# short
 *      NC_INT      C# int
 *      NC_FLOAT    C# float
 *      NC_DOUBLE   C# double
 *      NC_UBYTE    C# byte
 *      NC_USHORT   C# ushort
 *      NC_UINT     C# uint
 *      NC_INT64    C# long
 *      NC_UINT64   C# ulong
 *      NC_STRING   C# string
 * 
 * Additionally, the following *_var* functions do not have an exact netCDF data type
 * 
 *      text
 *      schar   
 *      uchar
 *      
 * NC_CHAR and NC_BYTE do not have an exact set of *_var* functions
 *  
 */

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CsNetCDF
{
    public static partial class NetCDF
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

        /* Control the compression
         */
        public const int NC_NODEFLATE = 0;
        public const int NC_DEFLATE = 1;

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
            // The following are use internally in support of user-defines
            // types. They are also the class returned by nc_inq_user_type.
            /// <summary>vlen (variable-length) types</summary>
            NC_VLEN = 13,
            /// <summary>opaque types</summary>
            NC_OPAQUE = 14,
            /// <summary>enum types</summary>
            NC_ENUM = 15,
            /// <summary>compound types</summary>
            NC_COMPOUND = 16
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
        /// NetCDF.nc_put_att_float(ncid, DataVarid, "_FillValue", NetCDF.nc_type.NC_FLOAT, 1, NetCDF.FillVars.FILL_FLOAT);
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
            /// <summary>Use diskless file. Mode flag for nc_open() or nc_create()</summary>
            NC_DISKLESS = 0x0008,
            /// <summary>Share updates, limit caching. Use this in mode flags for both nc_create() and nc_open()</summary>
            NC_SHARE = 0x0800,
            /// <summary>Read from memory. Mode flag for nc_open() or nc_create()</summary>
            NC_INMEMORY = 0x8000
        }
        #endregion

        #region Methods returning const char * that require the custom Marshaller
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

        #region CustomMarshaller
        // From https://stackoverflow.com/questions/6300093/why-cant-i-return-a-char-string-from-c-to-c-sharp-in-a-release-build
        class ConstCharPtrMarshaler : ICustomMarshaler
        {
            public object MarshalNativeToManaged(IntPtr pNativeData) => Marshal.PtrToStringAnsi(pNativeData);
            public IntPtr MarshalManagedToNative(object ManagedObj) => IntPtr.Zero;
            public void CleanUpNativeData(IntPtr pNativeData) { }
            public void CleanUpManagedData(object ManagedObj) { }
            public int GetNativeDataSize() => IntPtr.Size;
            static readonly ConstCharPtrMarshaler instance = new ConstCharPtrMarshaler();
            public static ICustomMarshaler GetInstance(string cookie) => instance;
        }
        #endregion
        #endregion

        #region File and Data IO
        //
        //  Some funtions are omitted here:
        //  nc_close_memio
        //  nc_create_par
        //  nc_create_par_fortran
        //  nc_def_user_format
        //  nc_int_user_format
        //  nc_open_mem
        //  nc_open_memio
        //  nc_open_par
        //  nc_open_par_fortran
        //  nc_var_par_access
        //
        /// <summary>Provided fpr completeness - No longer necessary for user to invoke manually.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_abort(int ncid);

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
        public static extern int nc_inq_path(int ncid, out IntPtr pathlen, StringBuilder path);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_type(int ncid, out nc_type type, StringBuilder name, out int size);

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
        public static extern int nc_def_dim(int ncid, string name, IntPtr len, out int dimidp);

        /// <summary>Find the name and length of a dimension.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_dim(int ncid, int dimid, StringBuilder name, out IntPtr len);

        /// <summary>Find the ID of a dimension from the name.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_dimid(int ncid, string name, out int dimid);

        /// <summary>Find the length of a dimension.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_dimlen(int ncid, int dimid, out IntPtr len);

        /// <summary>Find out the name of a dimension.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_dimname(int ncid, int dimid, StringBuilder name);

        /// <summary>Find the number of dimensions.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_ndims(int ncid, out int ndims);

        /// <summary>Find the ID of the unlimited dimension.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_unlimdim(int ncid, out int unlimdimid);

        /// <summary>Find the ID of the unlimited dimension.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_unlimdims(int ncid, int[] nunlimdimsp, int[] unlimdimidsp);

        /// <summary>Rename a dimension.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_rename_dim(int ncid, int dimid, string name, out int status);
        #endregion

        #region Defining Variables
        //
        // Defining Variables
        // Learning about Variables
        //
        /// <summary>Define a variable</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_var(int ncid, string name, nc_type xtype, int ndims, int[] dimids, out int varidp);

        /// <summary>Define fill value behavior for a variable. This must be done after nc_def_var</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_var_fill(int ncid, int varid, int no_fill, int fill_value);

        /// <summary>Set compression settings for a variable. Lower is faster, higher is better.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_var_deflate(int ncid, int varid, int shuffle, int deflate, int deflate_level);

        /// <summary>Set fletcher32 checksum for a var. This must be done after nc_def_var</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_var_fletcher32(int ncid, int varid, int fletcher32);

        /// <summary>Define chunking for a variable. This must be done after nc_def_var</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_var_chunking(int ncid, int varid, int storage, out int chunksizesp);

        /// <summary>Define endianness of a variable.
        /// NC_ENDIAN_NATIVE to select the native endianness of the platform (the default), NC_ENDIAN_LITTLE to use little-endian, NC_ENDIAN_BIG to use big-endian
        /// </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_var_endian(int ncid, int varid, int endian);

        /// <summary>Define a filter for a variable</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_var_filter(int ncid, int varid, uint id, int nparams, out uint parms);

        /// <summary>Set szip compression settings on a variable.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_set_var_szip(int ncid, int varid, int options_maskp, int pixels_per_blockp);

        /// <summary>Rename a variable.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_rename_var(int ncid, int varid, string name);

        /// <summary>Use this function to free resources associated with NC_STRING data.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_free_string(IntPtr len, IntPtr[] data);

        /// <summary>Set the per-variable cache size, nelems, and preemption policy. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_set_var_chunk_cache(int ncid, int varid, int size, int nelems, float preemption);

        /// <summary>Get the per-variable cache size, nelems, and preemption policy.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var_chunk_cache(int ncid, int varid, out int sizep, out int nelemsp, out float preemptionp);
        #endregion

        #region Reading Data from Variables (x86 and x64 versions)
        #region nc_get_var*
        //
        // Reading values from variables
        //  Note that the generic functions have been omitted:
        //  nc_get_var
        //  nc_get_vara
        //  nc_get_vars
        //  and all deprecated nc_get_varm funcrions
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
        #endregion

        #region get_var1
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_text(int ncid, int varid, IntPtr[] index, out byte ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_schar(int ncid, int varid, IntPtr[] index, out sbyte ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_uchar(int ncid, int varid, IntPtr[] index, out byte ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_short(int ncid, int varid, IntPtr[] index, out short ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_int(int ncid, int varid, IntPtr[] index, out int ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_long(int ncid, int varid, IntPtr[] index, out long ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_float(int ncid, int varid, IntPtr[] index, out float ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_double(int ncid, int varid, IntPtr[] index, out double ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_ubyte(int ncid, int varid, IntPtr[] index, out byte ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_ushort(int ncid, int varid, IntPtr[] index, out ushort ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_uint(int ncid, int varid, IntPtr[] index, out uint ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_longlong(int ncid, int varid, IntPtr[] index, out long ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_ulonglong(int ncid, int varid, IntPtr[] index, out ulong ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_var1_string(int ncid, int varid, IntPtr[] index, IntPtr[] ip);
        #endregion

        #region get_vara
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_text(int ncid, int varid, IntPtr[] start, IntPtr[] count, byte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_schar(int ncid, int varid, IntPtr[] start, IntPtr[] count, sbyte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_uchar(int ncid, int varid, IntPtr[] start, IntPtr[] count, byte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_short(int ncid, int varid, IntPtr[] start, IntPtr[] count, short[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_int(int ncid, int varid, IntPtr[] start, IntPtr[] count, int[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_long(int ncid, int varid, IntPtr[] start, IntPtr[] count, long[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_float(int ncid, int varid, IntPtr[] start, IntPtr[] count, float[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_double(int ncid, int varid, IntPtr[] start, IntPtr[] count, double[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_ubyte(int ncid, int varid, IntPtr[] start, IntPtr[] count, byte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_ushort(int ncid, int varid, IntPtr[] start, IntPtr[] count, ushort[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_uint(int ncid, int varid, IntPtr[] start, IntPtr[] count, uint[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_longlong(int ncid, int varid, IntPtr[] start, IntPtr[] count, long[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_ulonglong(int ncid, int varid, IntPtr[] start, IntPtr[] count, ulong[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vara_string(int ncid, int varid, IntPtr[] start, IntPtr[] count, IntPtr[] ip);
        #endregion

        #region get_vars
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_text(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, byte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_uchar(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, byte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_schar(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, sbyte[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_short(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, short[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_int(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, int[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_long(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, long[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_float(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, float[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_double(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, double[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_ushort(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, ushort[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_uint(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, uint[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_longlong(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, long[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_ulonglong(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, ulong[] ip);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vars_string(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, IntPtr[] ip);
        #endregion

        #endregion

        #region Learning about Variables
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_varid(int ncid, string name, out int varidp);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_var(int ncid, int varid, string name, out nc_type type, out int ndims, int[] dimids, out int natts);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_varname(int ncid, int varid, StringBuilder name);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_vartype(int ncid, int varid, out nc_type xtypep);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_varndims(int ncid, int varid, out int ndims);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_vardimid(int ncid, int varid, int[] dimids);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_varnatts(int ncid, int varid, out int nattsp);

        /// <summary>Find out compression settings of a var.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_var_deflate(int ncid, int varid, out int shufflep, out int deflatep, out int deflate_levelp);
        
        /// <summary>Inquire about fletcher32 checksum for a var.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_var_fletcher32(int ncid, int varid, out int fletcher32p);

        /// <summary>Inq chunking stuff for a var.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_var_chunking(int ncid, int varid, out int storagep, out int chunksizesp);

        /// <summary>Inq fill value setting for a var.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_var_fill(int ncid, int varid, out int no_fill, out int fill_valuep);

        /// <summary>Learn about the endianness of a variable.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_var_endian(int ncid, int varid, out int endianp);

        /// <summary>Find out szip settings of a var.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_var_szip(int ncid, int varid, out int options_maskp, out int pixels_per_blockp);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_nvars(int ncid, out int nvars);

        /// <summary>Learn about the filter on a variable</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_var_filter(int ncid, int varid, out uint idp, out int nparams, out uint parms);
        #endregion

        #region Writing variables
        #region nc_put_var
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
        #endregion

        #region put_var1
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_text(int ncid, int varid, IntPtr[] index, byte op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_schar(int ncid, int varid, IntPtr[] index, sbyte op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_uchar(int ncid, int varid, IntPtr[] index, byte op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_short(int ncid, int varid, IntPtr[] index, short op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_int(int ncid, int varid, IntPtr[] index, int op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_long(int ncid, int varid, IntPtr[] index, long op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_float(int ncid, int varid, IntPtr[] index, float op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_double(int ncid, int varid, IntPtr[] index, double op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_ubyte(int ncid, int varid, IntPtr[] index, byte op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_ushort(int ncid, int varid, IntPtr[] index, ushort op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_uint(int ncid, int varid, IntPtr[] index, uint op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_longlong(int ncid, int varid, IntPtr[] index, long op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_ulonglong(int ncid, int varid, IntPtr[] index, ulong op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_var1_string(int ncid, int varid, IntPtr[] index, string op);
        #endregion

        #region put_vara
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_text(int ncid, int varid, IntPtr[] start, IntPtr[] count, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_schar(int ncid, int varid, IntPtr[] start, IntPtr[] count, sbyte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_uchar(int ncid, int varid, IntPtr[] start, IntPtr[] count, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_short(int ncid, int varid, IntPtr[] start, IntPtr[] count, short[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_int(int ncid, int varid, IntPtr[] start, IntPtr[] count, int[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_long(int ncid, int varid, IntPtr[] start, IntPtr[] count, long[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_float(int ncid, int varid, IntPtr[] start, IntPtr[] count, float[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_double(int ncid, int varid, IntPtr[] start, IntPtr[] count, double[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_ubyte(int ncid, int varid, IntPtr[] start, IntPtr[] count, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_ushort(int ncid, int varid, IntPtr[] start, IntPtr[] count, ushort[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_uint(int ncid, int varid, IntPtr[] start, IntPtr[] count, uint[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_longlong(int ncid, int varid, IntPtr[] start, IntPtr[] count, long[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_ulonglong(int ncid, int varid, IntPtr[] start, IntPtr[] count, ulong[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vara_string(int ncid, int varid, IntPtr[] start, IntPtr[] count, string[] op);
        #endregion

        #region put_vars
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_text(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_uchar(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, byte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_schar(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, sbyte[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_short(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, short[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_int(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, int[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_long(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, long[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_float(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, float[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_double(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, double[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_ushort(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, ushort[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_uint(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, uint[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_longlong(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, long[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_ulonglong(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, ulong[] op);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vars_string(int ncid, int varid, IntPtr[] startp, IntPtr[] countp, IntPtr[] stridep, string op);
        #endregion

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_copy_var(int ncid_in, int varid, int ncid_out);
        #endregion

        #region Attributes 
        #region Learning about Attributes
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_att(int ncid, int varid, string name, out nc_type xtypep, out IntPtr lenp);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_attid(int ncid, int varid, string name, out int idp);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_attname(int ncid, int varid, int attnum, StringBuilder name);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_natts(int ncid, out int ngatts);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_atttype(int ncid, int varid, string name, out nc_type xtypep);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_attlen(int ncid, int varid, string name, out IntPtr lenp);

        #region x64
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_att(int ncid, int varid, string name, out nc_type xtypep, out long lenp);
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_attlen(int ncid, int varid, string name, out long lenp);
        #endregion
        #endregion

        #region Getting Attributes
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
        #endregion

        #region Writing Attributes
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_text(int ncid, int varid, string name, IntPtr len, string value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_schar(int ncid, int varid, string name, nc_type type, IntPtr len, sbyte[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_uchar(int ncid, int varid, string name, nc_type type, IntPtr len, byte[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_short(int ncid, int varid, string name, nc_type type, IntPtr len, short[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_int(int ncid, int varid, string name, nc_type type, IntPtr len, int[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_long(int ncid, int varid, string name, nc_type type, IntPtr len, long[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_float(int ncid, int varid, string name, nc_type type, IntPtr len, float[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_double(int ncid, int varid, string name, nc_type type, IntPtr len, double[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_ubyte(int ncid, int varid, string name, nc_type type, IntPtr len, byte[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_ushort(int ncid, int varid, string name, nc_type type, IntPtr len, ushort[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_uint(int ncid, int varid, string name, nc_type type, IntPtr len, uint[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_longlong(int ncid, int varid, string name, nc_type type, IntPtr len, long[] value);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_att_ulonglong(int ncid, int varid, string name, nc_type type, IntPtr len, ulong[] value);
        #endregion

        #region Copying, Deleting and Renaming Attributes
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_copy_att(int ncid_in, int varid_in, string name, int ncid_out, int varid_out);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_del_att(int ncid_in, int varid, string name);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_rename_att(int ncid, int varid, string name, string newname);
        #endregion
        #endregion

        #region Groups
        /// <summary>Define a new group.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_grp(int ncid, string name, out int grp_ncid);

        /// <summary>Retrieve a list of dimension ids associated with a group</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_dimids(int ncid, out int ndims, int[] dimids, int include_parents);

        /// <summary>Given a full name and ncid, find group ncid.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_grp_full_ncid(int ncid, string full_name, out int grp_ncid);

        /// <summary>Given a name and parent ncid, find group ncid.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_grp_ncid(int ncid, string grp_name, out int grp_ncid);

        /// <summary>Given an ncid, find the ncid of its parent group.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_grp_parent(int ncid, out int parent_ncid);

        /// <summary>Given locid, find name of group. (Root group is named "/".) </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_grpname(int ncid, StringBuilder name);

        /// <summary>
        /// Given ncid, find full name and len of full name. (Root group is named "/", with length 1.) 
        /// But use the C# friendlier nc_inq_grpname_full(ncid) instead
        /// </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_grpname_full(int ncid, out IntPtr lenp, StringBuilder full_name);

        /// <summary>Given ncid, find len of full name. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_grpname_len(int ncid, out IntPtr lenp);

        /// <summary>Given a location id, return the number of groups it contains, and an array of their locids.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_grps(int ncid, out int numgrps, out int ncids);

        /// <summary>Given an ncid and group name (NULL gets root group), return locid. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_ncid(int ncid, string name, out int grp_ncid);

        /// <summary>Retrieve a list of types associated with a group.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_typeids(int ncid, out int ntypes, int[] typeids);

        /// <summary>Get a list of varids associated with a group given a group ID.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_varids(int ncid, out int nvars, int[] varids);

        /// <summary>Rename a group.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_rename_grp(int ncid, string name);

        /// <summary>Print the metadata for a file.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_show_metadata(int ncid);
        #endregion


        // NOTE User defined, Compound, Enum and VLen functions have not yet been tested
        //  and the functions required for VLen are incomplete. e.g. the VLen struct is not defined here
        //  There is also a macro defined for VLen, which we do not have : #define NC_COMPOUND_OFFSET(S,M)    (offsetof(S,M))

        #region Untested functions

        #region User-Defined Types
        /// <summary> Get the name and size of a type. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_type(int ncid, nc_type xtype, StringBuilder name, out IntPtr size);

        /// <summary> Are two types equal? </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_type_equal(int ncid1, nc_type typeid1, int ncid2, nc_type typeid2, out int equal);

        /// <summary> Get the id of a type from the name. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_typeid(int ncid, string name, out nc_type typeidp);

        /// <summary> Find all user-defined types for a location. This finds all user-defined types in a group. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_typeids(int ncid, out int ntypes, out int typeids);

        /// <summary> Find out about a user defined type. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_user_type(int ncid, nc_type xtype, StringBuilder name, out IntPtr size, out nc_type base_nc_typep, out int nfieldsp, out int classp);
        #endregion

        #region Compound Types
        /// <summary> Here are functions for dealing with compound types.  Create a compound type. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_compound(int ncid, int size, string name, out nc_type typeidp);

        /// <summary> Insert a named field into a compound type. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_insert_compound(int ncid, nc_type xtype, string name, int offset, nc_type field_typeid);

        /// <summary> Insert a named array into a compound type. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_insert_array_compound(int ncid, nc_type xtype, string name, int offset, nc_type field_typeid, int ndims, int dim_sizes);

        /// <summary> Get the name, size, and number of fields in a compound type. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_compound(int ncid, nc_type xtype, StringBuilder name, out IntPtr sizep, out int nfieldsp);

        /// <summary> Get the name of a compound type. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_compound_name(int ncid, nc_type xtype, StringBuilder name);

        /// <summary> Get the size of a compound type. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_compound_size(int ncid, nc_type xtype, out IntPtr sizep);

        /// <summary> Get the number of fields in this compound type. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_compound_nfields(int ncid, nc_type xtype, out int nfieldsp);

        /// <summary> Given the xtype and the fieldid, get all info about it. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_compound_field(int ncid, nc_type xtype, int fieldid, StringBuilder name, out int offsetp, out nc_type field_typeidp, out int ndimsp, out int dim_sizesp);

        /// <summary> Given the typeid and the fieldid, get the name. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_compound_fieldname(int ncid, nc_type xtype, int fieldid, StringBuilder name);

        /// <summary> Given the xtype and the name, get the fieldid. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_compound_fieldindex(int ncid, nc_type xtype, string name, out int fieldidp);

        /// <summary> Given the xtype and fieldid, get the offset. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_compound_fieldoffset(int ncid, nc_type xtype, int fieldid, out int offsetp);

        /// <summary> Given the xtype and the fieldid, get the type of that field. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_compound_fieldtype(int ncid, nc_type xtype, int fieldid, out nc_type field_typeidp);

        /// <summary> Given the xtype and the fieldid, get the number of dimensions for that field (scalars are 0). </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_compound_fieldndims(int ncid, nc_type xtype, int fieldid, out int ndimsp);

        /// <summary> Given the xtype and the fieldid, get the sizes of dimensions for that field. User must have allocated storage for the dim_sizes. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_compound_fielddim_sizes(int ncid, nc_type xtype, int fieldid, out int dim_sizes);
        #endregion

        #region Enum types
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
        public static extern int nc_inq_enum(int ncid, nc_type xtype, StringBuilder name, out nc_type base_nc_typep, out IntPtr base_sizep, out int num_membersp);

        /// <summary>Get information about an enum member</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_enum_member(int ncid, nc_type xtype, int idx, string name, out object value);

        /// <summary>Get enum name from enum value. Name size will be <= NC_MAX_NAME.</summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_enum_ident(int ncid, nc_type xtype, long value, StringBuilder identifier);
        #endregion

        #region Variable Length Array Types
        /// <summary>* This is the type of arrays of vlens. * Calculate an offset for creating a compound type. This calls a mysterious C macro which was found carved into one of the blocks of the Newgrange passage tomb in County Meath, Ireland. This code has been carbon dated to 3200 B.C.E.  Create a variable length type. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_def_vlen(int ncid, string name, nc_type base_typeid, out nc_type xtypep);

        /// <summary> Find out about a vlen. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_inq_vlen(int ncid, nc_type xtype, StringBuilder name, out IntPtr datum_sizep, out nc_type base_nc_typep);

        /// <summary> When you read VLEN type the library will actually allocate the storage space for the data. This storage space must be freed, so pass the pointer back to this function, when you're done with the data, and it will free the vlen memory. </summary>
        //[DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int nc_free_vlen(nc_vlen_t* vl);

        //[DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int nc_free_vlens(int len, nc_vlen_t vlens[]);

        /// <summary> Put or get one element in a vlen array. </summary>
        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_put_vlen_element(int ncid, int typeid1, out object vlen_element, IntPtr len, object data);

        [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nc_get_vlen_element(int ncid, int typeid1, object vlen_element, out IntPtr len, out object data);
        #endregion
        #endregion

        #region Misc methods
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

    }
}
