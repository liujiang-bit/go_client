#import <Foundation/Foundation.h>

//获取安全距离
const char* _getSafeAreaInsets()
{
    NSString *rect = @"0, 0, 0, 0, 1";
    if(@available(iOS 11.0, *))
    {
        UIEdgeInsets safeAreaInset = UIApplication.sharedApplication.windows[0].safeAreaInsets;
        CGFloat scale = [UIScreen mainScreen].scale;
        rect = [[NSString alloc] initWithString:[NSString stringWithFormat:@"%d, %d, %d,%d, 1", (int)(safeAreaInset.left*scale), (int)(safeAreaInset.right*scale),(int)(safeAreaInset.top*scale), (int)(safeAreaInset.bottom*scale)]];
    }
    
    const char* output = [rect UTF8String];
    return strdup(output);
}
