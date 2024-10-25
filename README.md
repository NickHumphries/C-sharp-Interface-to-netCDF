# C-sharp-Interface-to-netCDF
A C# safe, managed interface to the netCDF DLL

netCDF: doi:10.5065/D6H70CW6 https://doi.org/10.5065/D6H70CW6
     
This C# interface supports the functions provided by the Unidata netcdf.dll (https://www.unidata.ucar.edu/software/netcdf/) 
currently up to 4.8, although not all functions are supported (e.g. I have omitted the deprecated varm* functions).

This interface is intended as a more complete replacement of that provided by Microsoft Research as a part of SDS but which has not been updated in line with netCDF developments by UniData. 

This file supports both x86 and x64 versions of the dlls, by defining the index[] start[] and count[] arrays for get_vara, get_var1 and get_vars methods as IntPtr.
Wrappers for these methods have been provided so that these arrays can always be defined in the calling program as int[].
Note that this also applies to lengths, such as returned from nc_inq_dimlen, where again the lengths are defined as IntPtr but wrappers exists to allow int to be used.
 
A collection of C# friendly methods have been provided to simplify calls to functions returning string variables and also for 
getting and putting attributes. Thanks to https://stackoverflow.com/questions/6300093/why-cant-i-return-a-char-string-from-c-to-c-sharp-in-a-release-build 
(https://stackoverflow.com/users/1164966/benoit-blanchon) for the custom marshaller.

Also, I have provided a couple of examples of how to get or put multidimensional arrays without reformatting. 
Any of the functions can be copied and modified to provide direct access to multidimensional arrays simply by changing the method from: 
 
          nc_put_var_float(int ncid, int varid, float[] op);
 
          to nc_put_var_float(int ncid, int varid, float[,] op);
          or nc_put_var_float(int ncid, int varid, float[,,] op);
        
I suggest putting these modified methods into the CSnetCDFExtensions.cs file, to keep the principal interface more closely focused on the netCDF C interface API.
                
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


Note that I have not yet implemented functions related to user defined, compound or variable length data types. I have not used these myself (nor encountered them in the datasets I work with) so I have not seen them as a priority. 
