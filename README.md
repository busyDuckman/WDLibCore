# WDLib

This is my 'toolbox' of c# code started in 2001 (since .net was in beta).
It's an eclectic set of things that help c# development.

I am migrating it to .netcore 2.0. and putting it on github, to act as a submodule
for other projects I intend to put on github. 
  
Defines:
  
    define UNSAFE to enable sections of code that can only compile with /unsafe
    define DISABLE_WINFORMS to disable winforms dependent code
    define WDLIB_LIGHT to prune novel stuff the increases the size of the library.
    
    
    //.netcore specific stuff has been seperated via: 
    #if NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0
    
    
See LICENSE.md for more details.