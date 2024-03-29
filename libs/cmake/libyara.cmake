# Minimum CMake required
cmake_minimum_required(VERSION 3.10)
set(yara_LIBYARA_SRC_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../yara/libyara")

set(yara_LIBYARA_INC
	${yara_LIBYARA_SRC_PATH}/include/yara/ahocorasick.h
	${yara_LIBYARA_SRC_PATH}/include/yara/arena.h
	${yara_LIBYARA_SRC_PATH}/include/yara/atoms.h
	${yara_LIBYARA_SRC_PATH}/include/yara/base64.h
	${yara_LIBYARA_SRC_PATH}/include/yara/bitmask.h
	${yara_LIBYARA_SRC_PATH}/include/yara/compiler.h
	${yara_LIBYARA_SRC_PATH}/include/yara/error.h
	${yara_LIBYARA_SRC_PATH}/include/yara/exec.h
	${yara_LIBYARA_SRC_PATH}/include/yara/exefiles.h
	${yara_LIBYARA_SRC_PATH}/include/yara/filemap.h
	${yara_LIBYARA_SRC_PATH}/include/yara/hash.h
	${yara_LIBYARA_SRC_PATH}/include/yara/integers.h
	${yara_LIBYARA_SRC_PATH}/include/yara/libyara.h
	${yara_LIBYARA_SRC_PATH}/include/yara/limits.h
	${yara_LIBYARA_SRC_PATH}/include/yara/mem.h
	${yara_LIBYARA_SRC_PATH}/include/yara/modules.h
	${yara_LIBYARA_SRC_PATH}/include/yara/notebook.h
	${yara_LIBYARA_SRC_PATH}/include/yara/object.h
	${yara_LIBYARA_SRC_PATH}/include/yara/parser.h
	${yara_LIBYARA_SRC_PATH}/include/yara/proc.h
	${yara_LIBYARA_SRC_PATH}/include/yara/re.h
	${yara_LIBYARA_SRC_PATH}/include/yara/rules.h
	${yara_LIBYARA_SRC_PATH}/include/yara/scan.h
	${yara_LIBYARA_SRC_PATH}/include/yara/scanner.h
	${yara_LIBYARA_SRC_PATH}/include/yara/sizedstr.h
	${yara_LIBYARA_SRC_PATH}/include/yara/stack.h
	${yara_LIBYARA_SRC_PATH}/include/yara/stopwatch.h
	${yara_LIBYARA_SRC_PATH}/include/yara/stream.h
	${yara_LIBYARA_SRC_PATH}/include/yara/strutils.h
	${yara_LIBYARA_SRC_PATH}/include/yara/threading.h
	${yara_LIBYARA_SRC_PATH}/include/yara/types.h
	${yara_LIBYARA_SRC_PATH}/include/yara/utils.h
	${yara_LIBYARA_SRC_PATH}/crypto.h
)

set(yara_LIBYARA_SRC
	${yara_LIBYARA_SRC_PATH}/grammar.y
	${yara_LIBYARA_SRC_PATH}/ahocorasick.c
	${yara_LIBYARA_SRC_PATH}/arena.c
	${yara_LIBYARA_SRC_PATH}/atoms.c
	${yara_LIBYARA_SRC_PATH}/base64.c
	${yara_LIBYARA_SRC_PATH}/bitmask.c
	${yara_LIBYARA_SRC_PATH}/compiler.c
	${yara_LIBYARA_SRC_PATH}/endian.c
	${yara_LIBYARA_SRC_PATH}/exec.c
	${yara_LIBYARA_SRC_PATH}/exefiles.c
	${yara_LIBYARA_SRC_PATH}/filemap.c
	${yara_LIBYARA_SRC_PATH}/hash.c
	${yara_LIBYARA_SRC_PATH}/hex_grammar.y
	${yara_LIBYARA_SRC_PATH}/hex_lexer.l
	${yara_LIBYARA_SRC_PATH}/lexer.l
	${yara_LIBYARA_SRC_PATH}/libyara.c
	${yara_LIBYARA_SRC_PATH}/mem.c
	${yara_LIBYARA_SRC_PATH}/modules.c
	${yara_LIBYARA_SRC_PATH}/notebook.c
	${yara_LIBYARA_SRC_PATH}/object.c
	${yara_LIBYARA_SRC_PATH}/parser.c
	${yara_LIBYARA_SRC_PATH}/proc.c
	${yara_LIBYARA_SRC_PATH}/re.c
	${yara_LIBYARA_SRC_PATH}/re_grammar.y
	${yara_LIBYARA_SRC_PATH}/re_lexer.l
	${yara_LIBYARA_SRC_PATH}/rules.c
	${yara_LIBYARA_SRC_PATH}/scan.c
	${yara_LIBYARA_SRC_PATH}/scanner.c
	${yara_LIBYARA_SRC_PATH}/sizedstr.c
	${yara_LIBYARA_SRC_PATH}/stack.c
	${yara_LIBYARA_SRC_PATH}/stopwatch.c
	${yara_LIBYARA_SRC_PATH}/strutils.c
	${yara_LIBYARA_SRC_PATH}/stream.c
	${yara_LIBYARA_SRC_PATH}/threading.c
	${yara_LIBYARA_SRC_PATH}/lexer.c
	${yara_LIBYARA_SRC_PATH}/hex_lexer.c
	${yara_LIBYARA_SRC_PATH}/grammar.c
	${yara_LIBYARA_SRC_PATH}/re_lexer.c
	${yara_LIBYARA_SRC_PATH}/hex_grammar.c
	${yara_LIBYARA_SRC_PATH}/re_grammar.c
)

if(EXISTS ${yara_LIBYARA_SRC_PATH}/modules/tests/)
	# Handle module options build
	if(EXISTS ${yara_LIBYARA_SRC_PATH}/modules/console/)
		set(yara_LIBYARA_MODULES
			${yara_LIBYARA_SRC_PATH}/modules/console/console.c
			${yara_LIBYARA_SRC_PATH}/modules/tests/tests.c
			${yara_LIBYARA_SRC_PATH}/modules/pe/pe.c
			${yara_LIBYARA_SRC_PATH}/modules/elf/elf.c
			${yara_LIBYARA_SRC_PATH}/modules/math/math.c
			${yara_LIBYARA_SRC_PATH}/modules/time/time.c
			${yara_LIBYARA_SRC_PATH}/modules/pe/pe_utils.c
		)
	else()		
		set(yara_LIBYARA_MODULES
			${yara_LIBYARA_SRC_PATH}/modules/tests/tests.c
			${yara_LIBYARA_SRC_PATH}/modules/pe/pe.c
			${yara_LIBYARA_SRC_PATH}/modules/elf/elf.c
			${yara_LIBYARA_SRC_PATH}/modules/math/math.c
			${yara_LIBYARA_SRC_PATH}/modules/time/time.c
			${yara_LIBYARA_SRC_PATH}/modules/pe/pe_utils.c
		)
	endif()
	
	# Handle module options build
	if(yara_CUCKOO_MODULE)
		add_definitions(-DCUCKOO_MODULE)
		set(yara_LIBYARA_MODULES ${yara_LIBYARA_MODULES} ${yara_LIBYARA_SRC_PATH}/modules/cuckoo/cuckoo.c)
	endif()

	if(yara_MAGIC_MODULE AND NOT WIN32)
		add_definitions(-DMAGIC_MODULE)
		set(yara_LIBYARA_MODULES ${yara_LIBYARA_MODULES} ${yara_LIBYARA_SRC_PATH}/modules/magic/magic.c)
	endif()

	if(yara_HASH_MODULE)
		add_definitions(-DHASH_MODULE)
		set(yara_LIBYARA_MODULES ${yara_LIBYARA_MODULES} ${yara_LIBYARA_SRC_PATH}/modules/hash/hash.c)
	endif()

	if(yara_DOTNET_MODULE)
		add_definitions(-DDOTNET_MODULE)
		set(yara_LIBYARA_MODULES ${yara_LIBYARA_MODULES} ${yara_LIBYARA_SRC_PATH}/modules/dotnet/dotnet.c)
	endif()

	if(yara_MACHO_MODULE)
		add_definitions(-DMACHO_MODULE)
		set(yara_LIBYARA_MODULES ${yara_LIBYARA_MODULES} ${yara_LIBYARA_SRC_PATH}/modules/macho/macho.c)
	endif()

	if(yara_DEX_MODULE)
		add_definitions(-DDEX_MODULE)
		set(yara_LIBYARA_MODULES ${yara_LIBYARA_MODULES} ${yara_LIBYARA_SRC_PATH}/modules/dex/dex.c)
	endif()
else()
	set(yara_LIBYARA_MODULES
		${yara_LIBYARA_SRC_PATH}/modules/tests.c
		${yara_LIBYARA_SRC_PATH}/modules/pe.c
		${yara_LIBYARA_SRC_PATH}/modules/elf.c
		${yara_LIBYARA_SRC_PATH}/modules/math.c
		${yara_LIBYARA_SRC_PATH}/modules/time.c
		${yara_LIBYARA_SRC_PATH}/modules/pe_utils.c
	)

	# Handle module options build
	if(yara_CUCKOO_MODULE)
		add_definitions(-DCUCKOO_MODULE)
		set(yara_LIBYARA_MODULES ${yara_LIBYARA_MODULES} ${yara_LIBYARA_SRC_PATH}/modules/cuckoo.c)
	endif()

	if(yara_MAGIC_MODULE AND NOT WIN32)
		add_definitions(-DMAGIC_MODULE)
		set(yara_LIBYARA_MODULES ${yara_LIBYARA_MODULES} ${yara_LIBYARA_SRC_PATH}/modules/magic.c)
	endif()

	if(yara_HASH_MODULE)
		add_definitions(-DHASH_MODULE)
		set(yara_LIBYARA_MODULES ${yara_LIBYARA_MODULES} ${yara_LIBYARA_SRC_PATH}/modules/hash.c)
	endif()

	if(yara_DOTNET_MODULE)
		add_definitions(-DDOTNET_MODULE)
		set(yara_LIBYARA_MODULES ${yara_LIBYARA_MODULES} ${yara_LIBYARA_SRC_PATH}/modules/dotnet.c)
	endif()

	if(yara_MACHO_MODULE)
		add_definitions(-DMACHO_MODULE)
		set(yara_LIBYARA_MODULES ${yara_LIBYARA_MODULES} ${yara_LIBYARA_SRC_PATH}/modules/macho.c)
	endif()

	if(yara_DEX_MODULE)
		add_definitions(-DDEX_MODULE)
		set(yara_LIBYARA_MODULES ${yara_LIBYARA_MODULES} ${yara_LIBYARA_SRC_PATH}/modules/dex.c)
	endif()
endif()


# Handle proc
# Actually cmake build system support windows linux and mac
set(yara_LIBYARA_PROC
	${yara_LIBYARA_SRC_PATH}/proc/windows.c
	${yara_LIBYARA_SRC_PATH}/proc/linux.c
	${yara_LIBYARA_SRC_PATH}/proc/mach.c
)

if (yara_ENABLE_PROFILING)
  add_definitions(-DYRPROFILING_ENABLED)
endif()

if (NOT BUILD_SHARED_LIB)
	# Create static library
	add_library(libyara ${yara_LIBYARA_SRC} ${yara_LIBYARA_INC} ${yara_LIBYARA_MODULES} ${yara_LIBYARA_PROC})
else()
	add_library(libyara SHARED ${yara_LIBYARA_SRC} ${yara_LIBYARA_INC} ${yara_LIBYARA_MODULES} ${yara_LIBYARA_PROC})
endif()


# Include directories management
target_include_directories(
	libyara
	PUBLIC ${yara_LIBYARA_SRC_PATH}/include
	PRIVATE ${yara_LIBYARA_SRC_PATH}
)

if(yara_CUCKOO_MODULE)
	# link with jansson lib
	include(jansson.cmake)
	target_link_libraries(libyara libjansson)
endif()

if(WIN32)
	add_definitions(-DUSE_WINDOWS_PROC)
	add_definitions(-DHAVE_WINCRYPT_H)		# not using openssl
	add_definitions(-D_CRT_SECURE_NO_WARNINGS) 	# maybe need to correct them
	# need to clean warnings
	add_definitions(
		/wd4005
		/wd4018
		/wd4090
		/wd4146
		/wd4244
		/wd4267
		/wd4996
	)
elseif(UNIX AND NOT APPLE)
	add_definitions(-DUSE_LINUX_PROC)
	target_link_libraries(libyara pthread m)
	if(yara_HASH_MODULE)
		find_package(OpenSSL REQUIRED)
		add_definitions(-DHAVE_LIBCRYPTO)
		target_link_libraries(libyara ${OPENSSL_LIBRARIES})
	endif()
elseif(APPLE)
	add_definitions(-DUSE_MACH_PROC)
endif()

install(TARGETS libyara LIBRARY DESTINATION lib ARCHIVE DESTINATION lib)
install(DIRECTORY ${yara_LIBYARA_SRC_PATH}/include DESTINATION include FILES_MATCHING PATTERN "*.h*")

include(GNUInstallDirs)
configure_file(${CMAKE_CURRENT_SOURCE_DIR}/yara.pc.in
               ${CMAKE_CURRENT_BINARY_DIR}/yara.pc @ONLY)

install(FILES ${CMAKE_CURRENT_BINARY_DIR}/yara.pc DESTINATION lib/pkgconfig)

