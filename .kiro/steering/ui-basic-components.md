---
inclusion: fileMatch
fileMatchPattern: '**/View_Mediator/**/*.cs'
---

基础 UI 组件 (C#/Skyunion)

当需要在 View 中绑定和操作 UI 组件时使用。

组件获取（在 View 的 InitUI 中）
```csharp
// 按钮
var btn = transform.Find("Path/To/Btn").GetComponent<Button>();
btn.onClick.AddListener(OnBtnClick);

// 文本
var text = transform.Find("Path/To/Text").GetComponent<Text>();
text.text = "内容";

// 图片
var image = transform.Find("Path/To/Image").GetComponent<Image>();

// Toggle
var toggle = transform.Find("Path/To/Toggle").GetComponent<Toggle>();
toggle.onValueChanged.AddListener(OnToggleChanged);

// Slider
var slider = transform.Find("Path/To/Slider").GetComponent<Slider>();
slider.onValueChanged.AddListener(OnSliderChanged);
```

多语言文本
```csharp
text.text = LanguageUtils.getText(languageId);
```

图片加载（Addressable/Sprite）
```csharp
// 通过 Skyunion 框架加载 Sprite
CoreUtils.assetService.LoadSprite(spriteName, (sprite) => {
    image.sprite = sprite;
});
```

资源路径常量
定义在 Assets/Scripts/Hotfix/MVC/RS.cs 中，如：
```csharp
RS.PlayerDefaultHeadIcon  // 默认头像
RS.ItemQualityBg[quality] // 道具品质背景
RS.HeroQualityBg[quality] // 英雄品质框
```
