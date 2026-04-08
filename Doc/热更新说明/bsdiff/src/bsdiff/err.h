#pragma once

/*
  原bsdiff.cpp和bspatch.cpp中均含有err、errx等函数
  所以全放到这个公共文件中
**/

inline int errx(int i, const char* str, ...) {
	va_list args;
	va_start(args, str);
	vprintf(str, args);
	va_end(args);
#ifdef WIN32
	printf("\r\nerror:%d", errno);
#endif
	return i;
}

inline int err(int i, const char* str, ...) {
	va_list args;
	va_start(args, str);
	vprintf(str, args);
	va_end(args);
#ifdef WIN32
	printf("\r\nerror:%d", errno);
#endif
	return i;
}