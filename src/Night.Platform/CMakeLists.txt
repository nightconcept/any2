cmake_minimum_required(VERSION 3.14)
project(NightPlatform C)

# Set flag for building a universal binary on macOS
if(APPLE)
	set(CMAKE_OSX_ARCHITECTURES "arm64;x86_64")
endif()

# Default to Release build for library
if(NOT CMAKE_BUILD_TYPE)
	set(CMAKE_BUILD_TYPE Release)
endif()

# Setup Foster Platform Lib
set(NIGHT_LIB_NAME NightPlatform)
add_library(${NIGHT_LIB_NAME} SHARED
	include/night_platform.h
	src/night_platform.c
)
target_include_directories(${NIGHT_LIB_NAME}
	PUBLIC $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
	PRIVATE $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/src>
)

# Setup MSVC/Windows Flags
if (MSVC)
    target_compile_options(${NIGHT_LIB_NAME} PRIVATE /W3)
    target_compile_options(${NIGHT_LIB_NAME} PRIVATE /WX)
endif()
if(WIN32)
    target_compile_definitions(${NIGHT_LIB_NAME} PRIVATE _UNICODE UNICODE _CRT_SECURE_NO_WARNINGS)
    target_compile_definitions(${NIGHT_LIB_NAME} PRIVATE NOMINMAX)
endif ()

# Get platform target
if (NIGHT_OVERRIDE_TARGET)
	set(NIGHT_TARGET ${NIGHT_OVERRIDE_TARGET})
elseif (APPLE)
	set(NIGHT_TARGET osx)
elseif (WIN32)
	set(NIGHT_TARGET win-x64)
elseif (UNIX)
	if(CMAKE_SYSTEM_PROCESSOR STREQUAL "aarch64")
		set(NIGHT_TARGET linux-arm64)
	elseif(CMAKE_SYSTEM_PROCESSOR STREQUAL "armv7l")
		set(NIGHT_TARGET linux-arm32)
	elseif(CMAKE_SYSTEM_PROCESSOR STREQUAL "armv6l")
		set(NIGHT_TARGET linux-arm32)
	else()
		set(NIGHT_TARGET linux-x64)
	endif()
else()
	message(FATAL_ERROR "Unknown Target!")
endif()

# Pull in SDL3 from Github, build it as a shared lib
if (NOT DEFINED NIGHT_SDL3_LIBS)
	include(FetchContent)
	set(FETCHCONTENT_QUIET FALSE)
	set(SDL_TEST_LIBRARY OFF)
	set(SDL_SHARED ON)
	set(SDL_STATIC OFF)

	FetchContent_Declare(
		SDL3
		GIT_REPOSITORY https://github.com/libsdl-org/sdl
		GIT_TAG release-3.2.14
		GIT_PROGRESS TRUE
	)
	FetchContent_MakeAvailable(SDL3)
	set(NIGHT_SDL3_LIBS SDL3-shared)
	set(NIGHT_SDL3_INCLUDE ${SDL3_SOURCE_DIRS}/include)
endif()

# Output main lib to the libs folder
set_target_properties(${NIGHT_LIB_NAME}
	PROPERTIES
	ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/libs/${NIGHT_TARGET}"
	ARCHIVE_OUTPUT_DIRECTORY_RELEASE "${CMAKE_CURRENT_SOURCE_DIR}/libs/${NIGHT_TARGET}"
	LIBRARY_OUTPUT_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/libs/${NIGHT_TARGET}"
	LIBRARY_OUTPUT_DIRECTORY_RELEASE "${CMAKE_CURRENT_SOURCE_DIR}/libs/${NIGHT_TARGET}"
	RUNTIME_OUTPUT_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/libs/${NIGHT_TARGET}"
	RUNTIME_OUTPUT_DIRECTORY_RELEASE "${CMAKE_CURRENT_SOURCE_DIR}/libs/${NIGHT_TARGET}"
)

# output SDL3 to the libs folder
set_target_properties(${NIGHT_SDL3_LIBS}
	PROPERTIES
	ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/libs/${NIGHT_TARGET}"
	ARCHIVE_OUTPUT_DIRECTORY_RELEASE "${CMAKE_CURRENT_SOURCE_DIR}/libs/${NIGHT_TARGET}"
	LIBRARY_OUTPUT_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/libs/${NIGHT_TARGET}"
	LIBRARY_OUTPUT_DIRECTORY_RELEASE "${CMAKE_CURRENT_SOURCE_DIR}/libs/${NIGHT_TARGET}"
	RUNTIME_OUTPUT_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/libs/${NIGHT_TARGET}"
	RUNTIME_OUTPUT_DIRECTORY_RELEASE "${CMAKE_CURRENT_SOURCE_DIR}/libs/${NIGHT_TARGET}"
)
