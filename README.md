# AutoUpdater 自动更新器

本程序用于更新软件

## 主要功能

+ 支持更新自身
+ 支持累计更新
+ 支持解压操作(7z,zip,rar)
+ 包含自动更新配置生成器,省去手动配置更新的烦恼

## 使用方法

1. [配置更新文件](#配置更新文件)
   + [或自动生成配置节点](#生成配置节点)
2. [修改部分代码](#修改部分代码)
3. 放置本软件在所需程序的目录下
4. 重命名文件为 文件名 v*.exe (* 为 yyMMdd)

## 配置更新文件

```xml
<config>

  <version value="V2.5">
    <item key="updateInfo" value="版本号:V2.5
1.更新说明

更新时间:2018.11.13">
    </item>
    <items>
        <it key="" value=""></it>
    </items>
  </version>

  <version value="V2.0">
    <item key="updateInfo" value="版本号:V2.0
1.更新说明

更新时间:2018.11.13">
    </item>
    <items>
        <it key="" value=""></it>
    </items>
  </version>

</config>
```

### 节点及属性说明

+ version: 更新版本号
+ item: 更新说明
+ it: 更新内容
  + key: 文件夹操作
    + add: 创建文件夹
    + remove: 移除文件夹
    + cut: 移动文件夹
    + copy: 复制文件夹
    + backup: 备份文件夹
    + restore: 还原文件夹
    + rename: 重命名文件夹
  + value: 文件操作
    + @: 下载文件
      + new: 更新更新器并启动
      + unzip: 下载并解压文件
    + remove: 移除文件
    + cut: 移动文件
    + copy: 复制文件
    + backup: 备份文件
    + restore: 还原文件
    + rename: 重命名文件

### 注意

+ 更新读取配置顺序是从下往上
+ 每一个it的属性key和属性value只能有一个有值
+ 更新配置如果只更新启动器请勿使用@new
+ 如果要更新子文件夹里的文件,则必须先创建子文件夹
+ 文件名尽量避免 + ,如实在无法避免,请采用下载不含有+的文件并重命名为含有+的文件
+ 不能删除任何以往更新配置(任一version节点)
+ version节点的value属性必须不一致
+ item节点的value属性首行必须为 xxx VXX.XX ,且Vxx.xx必须和相对应的version节点的value属性VXX.XX一致,其中 xxx 可在代码中配置(如下)

``` c#
private const string TipString = "版本号:";
```

### 示例

#### 文件夹操作

##### 创建文件夹

创建 一级文件夹

``` xml
<it key="一级文件夹@add" value=""></it>
```

在 一级文件夹 里面创建 二级文件夹

``` xml
<it key="一级文件夹\二级文件夹@add" value=""></it>
<it key="一级文件夹@add" value=""></it>
```

##### 移除文件夹

移除 一级文件夹 (包含里面全部文件以及子文件夹)

``` xml
<it key="一级文件夹@remove" value=""></it>
```

##### 移动文件夹

移动 一级文件夹 至 新文件夹

``` xml
<it key="一级文件夹@cut@新文件夹" value=""></it>
```

##### 复制文件夹

复制 一级文件夹 至 新文件夹

``` xml
<it key="一级文件夹@copy@新文件夹" value=""></it>
```

##### 备份文件夹

备份 一级文件夹 (备份至 Backup 目录对应位置)

``` xml
<it key="一级文件夹@backup" value=""></it>
<it key="Backup@add" value=""></it>
```

备份 一级文件夹\二级文件夹 (备份至 Backup 目录对应位置)

``` xml
<it key="一级文件夹\二级文件夹@backup" value=""></it>
<it key="Backup\一级文件夹@add" value=""></it>
<it key="Backup@add" value=""></it>
```

##### 还原文件夹

还原 一级文件夹 (从 Backup 目录还原至原位置)

``` xml
<it key="一级文件夹@restore" value=""></it>
```

##### 重命名文件夹

将 一级文件夹 重命名为 一

``` xml
<it key="一级文件夹@rename@一" value=""></it>
```

#### 文件操作

##### 下载文件

从 服务器更新文件存放目录\说明.txt 下载至 说明.txt

``` xml
<it key="" value="说明.txt@说明.txt"></it>
```

从 服务器更新文件存放目录\一级文件夹\说明.txt 下载至 一级文件夹\说明.txt

``` xml
<it key="" value="一级文件夹\说明.txt@说明.txt"></it>
<it key="一级文件夹@add" value=""></it>
```

从 服务器更新文件存放目录\KentCraft启动器 v190615.exe 下载至 程序根目录 并启动最新更新器

``` xml
<it key="" value="KentCraft启动器 v190615.exe@new@KentCraft启动器 v190615.exe"></it>
```

从 服务器更新文件存放目录\更新压缩包.zip 下载并解压至 程序根目录 (不包含压缩文件名)

``` xml
<it key="" value="更新压缩包.zip@更新压缩包.zip@unzip"></it>
```

##### 移除文件

移除 说明.txt

``` xml
<it key="" value="说明.txt@remove"></it>
```

移除程序目录下 md5 为 D41D8CD98F00B204E9800998ECF8427E 的文件

``` xml
<it key="" value="D41D8CD98F00B204E9800998ECF8427E@remove"></it>
```

移除 一级文件夹 下 md5 为 D41D8CD98F00B204E9800998ECF8427E 的文件

``` xml
<it key="" value="一级文件夹\D41D8CD98F00B204E9800998ECF8427E@remove"></it>
```

##### 移动文件

移动 说明.txt 至 一级文件夹

``` xml
<it key="" value=""></it>
```

##### 复制文件

复制 说明.txt 至 一级文件夹

``` xml
<it key="" value="说明.txt@copy@一级文件夹\说明.txt"></it>
```

##### 备份文件

备份 说明.txt (备份至 Backup 目录)

``` xml
<it key="" value="说明.txt@backup"></it>
<it key="Backup@add" value=""></it>
```

备份 一级文件夹\说明.txt (备份至 Backup 目录对应位置)

``` xml
<it key="" value="一级文件夹\说明.txt@backup"></it>
<it key="Backup\一级文件夹@add" value=""></it>
<it key="Backup@add" value=""></it>
```

##### 还原文件

还原 说明.txt (从 Backup 目录还原至原位置)

``` xml
<it key="" value="说明.txt@restore"></it>
```

##### 重命名文件

将 说明.txt 重命名为 说明文件.txt

``` xml
<it key="" value="说明.txt@rename@说明文件.txt"></it>
```

将 md5 为 A301B39E88A2347EABE07B3D931581A8 的文件重命名为 说明文件.txt

``` xml
<it key="" value="A301B39E88A2347EABE07B3D931581A8@rename@说明文件.txt"></it>
```

将 一级文件夹 下 md5 为 A301B39E88A2347EABE07B3D931581A8 的文件重命名为 说明文件.txt

``` xml
<it key="" value="一级文件夹\A301B39E88A2347EABE07B3D931581A8@rename@说明文件.txt"></it>
```

## 修改部分代码

``` c#
"KentCraft启动器 v*.exe" => "所需更新器名 v*.exe"
```

``` c#
"KentCraft启动器 v" => "所需更新器名 v"
```

``` c#
private const string ServerAddress = ""; //更新文件存放目录
private const string ConfigPath = ""; //更新配置文件存放路径
```

``` c#
foreach (Process item in pcs)
{
    if (item.ProcessName.Contains("KentCraft")) //所需程序进程名
    {
        if (item.Id != Process.GetCurrentProcess().Id)
            item.Kill();
    }
}
```

``` c#
au.OpenUpdate("KentCraft.exe"); //所需程序文件名
```

---

## 自动更新生成器

### 生成配置节点

1. 进入 KentCraft自动更新生成器 目录
2. 复制 样本 文件夹 并黏贴至 KentCraft自动更新生成器\bin\Debug 目录
3. 将复制的文件夹重命名为当天日期 (格式:yyMMdd)
4. 将要进行更新,下载,备份,还原的文件和文件夹以及根据md5重命名的文件扔到该文件夹里的对应目录
5. 打开 KentCraft自动更新生成器.exe
6. 修改 版本号 以及 更新日志
7. 点击 生成
8. 复制 最终代码 并黏贴到 配置文件的 config 节点里的最上方

## 说明

如要根据文件名删除文件请取消勾选 md5
