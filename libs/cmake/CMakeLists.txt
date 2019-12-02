# Minimum CMake required
cmake_minimum_required(VERSION 3.10)

# Project
project(yara LANGUAGES C VERSION 3.9.0)

# Config
set_property(GLOBAL PROPERTY USE_FOLDERS ON)

set(EXECUTABLE_OUTPUT_PATH ${CMAKE_BINARY_DIR}/bin)
set(LIBRARY_OUTPUT_PATH ${CMAKE_BINARY_DIR}/bin)
set(CMAKE_SHARED_LIBRARY_PREFIX "")


# Build options
option(yara_BUILD_YARA "Build yara executable" ON)
option(yara_BUILD_YARAC "Build yarac (compiler) executable" ON)
option(yara_BUILD_TESTS "Build tests" ON)

if (NOT BUILD_SHARED_LIB)
	message(STATUS "Shared lib: OFF")
else()
	message(STATUS "Shared lib: ON")

	SET(yara_BUILD_YARA OFF)
	SET(yara_BUILD_YARAC OFF)
	SET(yara_BUILD_TESTS OFF)
	SET(BUILD_SHARED_LIBS "TRUE")
	
	if(WIN32)
		message(STATUS "Explicit Windows export: ON")
		SET(CMAKE_WINDOWS_EXPORT_ALL_SYMBOLS "TRUE")
	endif()
endif()

# Module options
option(yara_ALL_MODULES "Include all yara modules" OFF)
option(yara_PE_MODULE "Build PE module for libyara" OFF)
option(yara_CUCKOO_MODULE "Build cuckoo module for libyara" OFF)
option(yara_MAGIC_MODULE "Build magic module for libyara" OFF)
option(yara_HASH_MODULE "Build hash module for libyara" OFF)
option(yara_DOTNET_MODULE "Build dotnet module for libyara" OFF)
option(yara_MACHO_MODULE "Build macho module for libyara" OFF)
option(yara_DEX_MODULE "Build dex module for libyara" OFF)

if (yara_ALL_MODULES)
	set(yara_PE_MODULE ON)
	set(yara_CUCKOO_MODULE ON)
	set(yara_MAGIC_MODULE ON)
	set(yara_HASH_MODULE ON)
	set(yara_DOTNET_MODULE ON)
	set(yara_MACHO_MODULE ON)
	set(yara_DEX_MODULE ON)
endif()

if(UNIX)
	option(yara_ADDRESS_SANITIZER "Enable address sanitizer" OFF)
	if(yara_ADDRESS_SANITIZER)
		SET(CMAKE_C_FLAGS  "${CMAKE_C_FLAGS} -fsanitize=address")
	endif()
endif()

# Build of yara
include(libyara.cmake)

if(yara_BUILD_YARA)
	include(yara.cmake)
endif()

if(yara_BUILD_YARAC)
	include(yarac.cmake)
endif()

if(yara_BUILD_TESTS)
	enable_testing()
	include(yaratest.cmake)
endif()

# Summary
message(STATUS "Configuration summary")
message(STATUS "Project name : ${PROJECT_NAME}")
message(STATUS "Project version : ${PROJECT_VERSION}")
message(STATUS "Build as shared lib : ${BUILD_SHARED_LIBS}")
message(STATUS "Build yara executable : ${yara_BUILD_YARA}")
message(STATUS "Build yarac (compiler) executable : ${yara_BUILD_YARAC}")
message(STATUS "Build with module PE : ${yara_PE_MODULE}")
message(STATUS "Build with module Cuckoo : ${yara_CUCKOO_MODULE}")
message(STATUS "Build with module Magic : ${yara_MAGIC_MODULE}")
message(STATUS "Build with module Hash : ${yara_HASH_MODULE}")
message(STATUS "Build with module Dotnet : ${yara_DOTNET_MODULE}")
message(STATUS "Build with module Macho : ${yara_MACHO_MODULE}")
message(STATUS "Build with module Dex : ${yara_DEX_MODULE}")
message(STATUS "Build tests : ${yara_BUILD_TESTS}")
