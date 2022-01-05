pushd %0\..

if not exist "yara/build/x86" (mkdir yara/build/x86)
cd yara/build/x86
del * /F /Q /S
cmake -G "Visual Studio 16 2019" ..\..\..\libs\cmake -DBUILD_SHARED_LIB=ON -D yara_ALL_MODULES=ON -A Win32
cmake --build . --config release

popd

if not exist "yara/build/x64" (mkdir yara/build/x64)
cd yara/build/x64
del * /F /Q /S
cmake -G "Visual Studio 16 2019" ..\..\..\libs\cmake -DBUILD_SHARED_LIB=ON -D yara_ALL_MODULES=ON -A x64
cmake --build . --config release