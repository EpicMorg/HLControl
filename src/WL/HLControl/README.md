# Half Life Style Controls

Well, you can use this just follow [GPL-3.0](https://github.com/gordonwalkedby/HLControl/blob/master/LICENSE). But if you use it for commercial purposes, contact me first.  
非商业用途请遵守 [GPL-3.0](https://github.com/gordonwalkedby/HLControl/blob/master/LICENSE)，商业用途请先联系我。  

This will need .NET Framework 4.5+ to work and only works in WinForm.  
需要 .NET 4.5 以上，而且只能在 WinForm 里使用。

Controls:   
目前支持的控件列表：  

1. Button
2. CheckBox
3. ComboBox
4. Form
5. HScrollBar
6. Label
7. ListBox
8. ListView
9. Panel
10. ProgressBar
11. RadioButton
12. TabsHeader
13. TextBox
14. TrackBar
15. VScrollBar

Though their names and properties were written in English, a lot of things in code are still in Chinese. So if you want to edit it, it might still be a difficult thing. Please don't ask me to write them in English all, I do love writing code in Chinese for now.   
尽管他们的名字、属性都是英文的，但是代码里面很多地方还是中文写的。所以编辑起来可能会很麻烦，但请不要让我用全英文写代码，我现在还是挺享受写中文的。  

The controls' code files are [here](https://github.com/gordonwalkedby/HLControl/tree/master/WL/HLControl).  
控件部分的代码文件在[这里](https://github.com/gordonwalkedby/HLControl/tree/master/WL/HLControl)。  

## Get DLL 获得DLL：

You can get the DLL file first, I named it as `WL.dll`.  
你可以从以下途径获得最新的DLL，DLL文件叫 `WL.dll`。

- [GitHub Release](https://github.com/gordonwalkedby/HLControl/releases)
- Nuget is no longer supported.

## Usage 用法：

Add my dll into your project's reference.  
把我的 DLL 加入到你的项目引用当中。  
![](https://s2.ax1x.com/2019/02/12/kwhsEj.png)  

Open you WinForm Designer's toolbox, create a new folder and name it whatever you want. Drag my dll from Windows Explorer into the toolbox folder. You will get the controls.  
打开 WinForm 编辑器的工具箱，新建一个选项卡，给一个名字。接着把我的DLL从文件管理器直接拖进去，你应该就可以看见控件了。  

![](https://s2.ax1x.com/2019/02/12/kwh4rF.gif)

Then open form's designer file, and edit it like this.  
接着打开窗体的设计文件，修改成这样。  

C# ：
```cs
namespace WindowsFormsApp1
{
    public partial class Form1 : WL.HLControl.HLForm
    {
        public Form1()
```

VB：
```vb
Partial Class Form1
    Inherits WL.HLControl.HLForm
```

And, you must do this!!! **Change the form's AutoScaleMode into None**, because my form will scale its size automatically in high DPI.  
然后，**一定要把窗体的 AutoScaleMode 改成 None**，因为我做的窗体会自己根据DPI缩放。
 
![](https://s2.ax1x.com/2019/02/12/kw4uIs.png)

Now, you can start your work.  
现在你可以真正开始了。  

![](https://s2.ax1x.com/2019/02/12/kw4yLD.png)

# Controls' details 各个控件的一些细节说明：  

## Label  
Four different states of it:Enabled, not Enabled, Highlight, Lowlight.and when one label highlighted, the others near it will be not highlighted.    
它有四种状态：启用、禁用、高亮、黯淡。并且如果一个变得高亮了，那么它旁边的就会取消高亮。    
![](https://s2.ax1x.com/2019/02/13/k0tyvV.png)

## Button  
Its height is fixed by the font size, just use it like a normal button.  
按钮的高度是根据字体大小固定的，其他的和一个普通按钮一样。  
![](https://s2.ax1x.com/2019/02/13/k0Y5X8.png)

## CheckBox  
Its size is fixed by the text, the property Checked is its value.  
大小是根据字体和文本长度固定的，属性 Checked 就是他的值。  
![](https://s2.ax1x.com/2019/02/13/k0YXpq.png)

## ListBox
Unlike the original listbox, its Items is a list(of string), you can only set and get string from it.Property SmallChange means how many lines moves when the mouse scroll once. ShowScrollBar means if it shows the scrollbar, if not ,you can still use the mouse scroll.     
和原版的不一样，这个 Items 是 list(of string) ，只能填入和输出字符串。属性 SmallChange 改变的是鼠标滚动一次的行数。ShowScrollBar 属性表示显示还是隐藏滚动条，如果隐藏也还可以用鼠标滚轮。    
![](https://s2.ax1x.com/2019/02/13/k0tj5d.png)

## ComboBox  
Unlike the original comboxbox, its Items is a list(of string), you can only set and get string from it.Property SmallChange means how many lines moves when the mouse scroll once. HighLightLabel means the label which will be highlighted when the menu is opened.in fact, when you click it, a ListBox popup for you to select.    
和原版的不一样，这个 Items 是 list(of string) ，只能填入和输出字符串。属性 SmallChange 改变的是鼠标滚动一次的行数。HighLightLabel 属性指的是这个控件激活的时候会高亮的 Label 。事实上，当你点击控件的时候，弹出来的是一个 ListBox 。   
![](https://s2.ax1x.com/2019/02/13/k0tSnU.png)  

## HScrollBar VScrollBar
Maximum, Minimum, Value ,Smallchange, just use it like the original one. But there're two new subs(void function) in it. PerformMouseWheel can put a MouseEventArgs into it to make it scroll. ChangeValueWithoutRaiseEvent can change its value without raise the event ValueChanged.  
Maximum, Minimum, Value ,Smallchange ，就像原版的滚动条控件一样使用就好了，但是有两个新函数在里面。PerformMouseWheel 可以传入 MouseEventArgs 来使它滚动，ChangeValueWithoutRaiseEvent 可以修改值但不会触发 ValueChanged 事件。  

![](https://s2.ax1x.com/2019/02/13/k0amuV.png)  

## ProgressBar
Just use it as the original one.And a new property AutoReset, which means it will reset value to Minimum when it reaches Maximum.    
就像原版一样使用就好了，但是有一个新属性 AutoReset ，意思是到达最大值之后就返回最小值。  
![](https://s2.ax1x.com/2019/02/13/k0aQN4.png)  

## TrackBar
Just use it as the original one. But it also has HighLightLabel and ChangeValueWithoutRaiseEvent.  
和原版的差不多，但是这个也有 HighLightLabel 属性和 ChangeValueWithoutRaiseEvent 函数。  
![](https://s2.ax1x.com/2019/02/13/k0a5Cj.png)  

## RadioButton
Checked and CheckedChanged.  
和原版的差不多。  
![](https://s2.ax1x.com/2019/02/13/k0dNzn.png)

## TextBox
Only one scrollbar in it, and always word wrap. And the selected color is still the Windows blue, the menu is also the original menu. Support HighLightLabel.  
只有一条滚动条，而且强制自动换行，并且选中的颜色和默认右键菜单依然是原版菜单。还有 HighLightLabel 属性。   
![](https://s2.ax1x.com/2019/02/13/k0dxw8.png)

## Panel
Just a half life styled panel. AutoScroll is my own autoscroll, and only supports vertical scroll. You can use ResetScroll() and PerformScroll(value As Integer) to scroll it in code.  
只是一个半条命风格化的 Panel。AutoScroll 是我自己的做的版本，只支持垂直滚动，你可以在代码里使用 ResetScroll() 和 PerformScroll(value As Integer) 来滚动它。   
![](https://s2.ax1x.com/2019/02/19/kcomSU.png)  

## ListView  
Well, it can't support groups.But support Columns and Items. And you can't add them in Designer, you need to do it in code like this.  
这个就不支持组了，只支持列 Columns 和行 Items ，而且也不能在设计器里直接添加，得写在代码里添加。  
```cs
private void Form1_Load(object sender, EventArgs e)
{
    hlListView1.Columns.Add("first");
    hlListView1.Columns.Add(new WL.HLControl.HLListViewColumn("second"));
    hlListView1.Columns.Add(new WL.HLControl.HLListViewColumn("thirt",200)); //200 is width of the column
    hlListView1.Items.Add(new WL.HLControl.HLListViewItem("title1", "aa", "bb","33"));
    hlListView1.Items.Add("only title");
}
```
Result will look like this.  
结果大概就是图中这样。  
![](https://s2.ax1x.com/2019/02/13/k0BZiF.png)  
You can also use these functions to add Columns and Items. You must add at least one column, or nothing will show up.    
你也可以用下面的函数添加行和列，而且必须最少有一列，否则什么都不显示。  
```cs
public void AddColumn(string name);
public void AddColumn(string name, uint width);
public void AddItem(string title, params string[] str);
```

## TabsHeader
yes, it is only a header, you need another real TabControl to work. It's seted in property RealTabControl. And you need to use TabHeaderWidth to control every tab's width.    
是的，这真的只是一个头部，你还得需要一个真的 TabControl 才能工作。这个在属性 RealTabControl 里设置。还需要 TabHeaderWidth 来控制每个标签的宽度。  
![](https://s2.ax1x.com/2019/02/13/k0BBeP.png)  
When the program starts , the real tabcontrol will move and resize to fit the header like this.  
程序开始后，真正的 tabcontrol 就会移动并且重新调整宽度来搭配 TabsHeader 。

![](https://s2.ax1x.com/2019/02/13/k06MEq.png)

## GroupList
Just like a listview but only supports groups. To add items, you have to write these codes. You must add at least one group, or nothing will show up.   
就像是一个 LiewView ，但是只支持组。要在里面添加东西，你必须写这些代码。最少必须添加至少一个组才能显示东西。  
![](https://s2.ax1x.com/2019/02/16/ksQmrD.png)

If you want to edit it , you'd better use these functions.  
如果要修改里面的内容，最好用这些函数。

```vb
HlGroupList1.GetGroup("aaa").Items.Add(New HLGroupItem("what?"))
HlGroupList1.GetGroup("aaa").GetItem("2").Ico = Nothing
```

