#ifndef NIGHT_PLATFORM_H
#define NIGHT_PLATFORM_H

// Export API
#if defined _WIN32 || defined __CYGWIN__
	#define NIGHT_API __declspec(dllexport)
	#define NIGHT_CALL __cdecl
#elif __GNUC__
	#define NIGHT_API __attribute__((__visibility__("default")))
	#define NIGHT_CALL
#else
	#define NIGHT_API
	#define NIGHT_CALL
#endif

typedef unsigned char NightBool;
typedef struct NightFont NightFont;
typedef void (NIGHT_CALL * NightWriteFn)(void *context, void *data, int size);

typedef enum NightImageWriteFormat
{
	NIGHT_IMAGE_WRITE_FORMAT_PNG,
	NIGHT_IMAGE_WRITE_FORMAT_QOI,
} NightImageWriteFormat;

#if __cplusplus
extern "C" {
#endif

NIGHT_API unsigned char* NightImageLoad(const unsigned char* memory, int length, int* w, int* h);

NIGHT_API void NightImageFree(unsigned char* data);

NIGHT_API NightBool NightImageWrite(NightWriteFn* func, void* context, NightImageWriteFormat format, int w, int h, const void* data);

NIGHT_API NightFont* NightFontInit(unsigned char* data, int length);

NIGHT_API void NightFontGetMetrics(NightFont* font, int* ascent, int* descent, int* linegap);

NIGHT_API int NightFontGetGlyphIndex(NightFont* font, int codepoint);

NIGHT_API float NightFontGetScale(NightFont* font, float size);

NIGHT_API float NightFontGetKerning(NightFont* font, int glyph1, int glyph2, float scale);

NIGHT_API void NightFontGetCharacter(NightFont* font, int glyph, float scale, int* width, int* height, float* advance, float* offsetX, float* offsetY, int* visible);

NIGHT_API void NightFontGetPixels(NightFont* font, unsigned char* dest, int glyph, int width, int height, float scale);

NIGHT_API void NightFontFree(NightFont* font);

#if __cplusplus
}
#endif

#endif
