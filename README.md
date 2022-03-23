# VRchat-MusicPlayer
## 介绍
这是一个用在VRChat World而制作的一个本地音乐播放器，可以支持一键添加部署歌曲,支持正常音乐播放器的功能
![](https://raw.githubusercontent.com/CCYellowStar/pic/master/20220317/QQ截图20220323144455.os3lqi5o58g.webp)
![](https://raw.githubusercontent.com/CCYellowStar/pic/master/20220317/QQ截图20220323144415.4mtttasa1fa0.webp)
## 用法
需要导入[vrchat世界sdk](https://vrchat.com/home/download)和[Udonsharp](https://github.com/vrchat-community/UdonSharp)  
将unity包拖入工程，然后先将`Assets\MusicPlayerMaker`里的`MusicPlayer.prefab`导入到场景并放好，可根据自己喜好重新修改外观  
再点开unity里`Windows`菜单里的`MusicPlayerMaker`配置菜单，然后把场景里的`MusicPlayer`拖进音乐播放器栏（注意名字不能改）再进行导入歌曲。
## 注意
当你导入完歌曲有可能会出现点播放后`MusicPlayer`的udon主脚本里`Music`音频数组的内容被还原成上一次的情况（如果点了歌曲的按钮没播放出来就去检查下这里），这是因为udonsharp刷新机制的问题，
试了几个办法我还是不能在代码上解决    
但是手动解决也很简单，当出现音频组内容还原到上一次后，关闭播放后先点一下重新排序还原一下`Music`音频组，然后对音频组进行展开再合上即可完成刷新。  
![](https://raw.githubusercontent.com/CCYellowStar/pic/master/20220317/image.3bn116rm8l80.webp)
