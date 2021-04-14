# C-sharp-Interface-to-netCDF
A C# safe, managed interface to the netCDF DLL

# C-sharp-Interface-to-netCDF
A C# safe, managed interface to the netCDF DLL

netCDF: doi:10.5065/D6H70CW6 https://doi.org/10.5065/D6H70CW6
     
     
This C# interface supports the functions provided by the Unidata netcdf.dll (https://www.unidata.ucar.edu/software/netcdf/) 
currently up to 4.7.4, although not all functions are supported (e.g. I have omitted the deprecatedvarm* functions)
 
This file supports both x86 and x64 versions of the dlls, the principal difference being that the index[] start[] and count[] 
arrays for functions such as get_var1 or get_vara are passed as int[] for x86 and long[] for x64.
 
A collection of C# friendly methods have been provided to simplify calls to functions returning string variables and also for 
getting and putting attributes. Thanks to https://stackoverflow.com/questions/6300093/why-cant-i-return-a-char-string-from-c-to-c-sharp-in-a-release-build 
(https://stackoverflow.com/users/1164966/benoit-blanchon) for the custom marshaller.

Also, I have provided a couple of examples of how to get or put multidimensional arrays without reformatting. 
Any of the functions can be copied and modified to provide direct access to multidimensional arrays simply by changing the method from: 
 
          nc_put_var_float(int ncid, int varid, float[] op);
 
          to nc_put_var_float(int ncid, int varid, float[,] op);
          or nc_put_var_float(int ncid, int varid, float[,,] op);
                
Data types
Some of the data types supported by the netCDF dll do not map exactly to C# data types
The following netCDF data types are defined:
             
      NC_BYTE     C# sbyte
      NC_CHAR     C# byte
      NC_SHORT    C# short
      NC_INT      C# int
      NC_FLOAT    C# float
      NC_DOUBLE   C# double
      NC_UBYTE    C# byte
      NC_USHORT   C# ushort
      NC_UINT     C# uint
      NC_INT64    C# long
      NC_UINT64   C# ulong
      NC_STRING   C# string
 
Additionally, the following_var* functions do not have an exact netCDF data type
 
      text
      schar   
      uchar
      
Also NC_CHAR and NC_BYTE do not have an exact set of_var* functions

