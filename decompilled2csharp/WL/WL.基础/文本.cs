using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace WL.基础
{
	/// <summary>
	/// 文本处理模块
	/// </summary>
	[StandardModule]
	public sealed class 文本
	{
		/// <summary>
		/// Base64加密解密类
		/// </summary>
		public sealed class Base64
		{
			protected Base64()
			{
			}

			/// <summary>
			/// 对字节数组进行加密
			/// </summary>
			public static string 加密字节数组(byte[] 字节数组)
			{
				if (类型.为空(字节数组))
				{
					return "";
				}
				return Convert.ToBase64String(字节数组);
			}

			/// <summary>
			/// 对字符串进行加密
			/// </summary>
			public static string 加密文本(string 文本, Encoding 编码 = null)
			{
				return 加密字节数组(文本转字节数组(文本, 编码));
			}

			/// <summary>
			/// 对Base64字符串进行解密，解密到字节数组
			/// </summary>
			public static byte[] 解密字节数组(string B64)
			{
				byte[] array = null;
				if (B64.Length > 0)
				{
					try
					{
						return Convert.FromBase64String(B64);
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						程序.出错(ex2);
						ProjectData.ClearProjectError();
					}
				}
				return null;
			}

			/// <summary>
			/// 对Base64字符串进行解密，解密到字符串
			/// </summary>
			public static string 解密文本(string B64, Encoding 编码 = null)
			{
				return 字节数组转文本(解密字节数组(B64), 编码);
			}
		}

		/// <summary>
		/// 一些比原版更好的正则处理，规则为大小写、多行模式，\r和\n都可以表示换行，无需特别注意
		/// </summary>
		public sealed class 正则
		{
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private static RegexOptions _Rule;

			private static RegexOptions Rule
			{
				get;
				set;
			} = RegexOptions.IgnoreCase | RegexOptions.Multiline;


			protected 正则()
			{
			}

			private static string FixRN(ref string p)
			{
				p = 文本.替换(p, "\\r", "\\n", "\\n\\n", "\\n", "\\n", "\\r\\n", "\\r\\n", "\r\n");
				return p;
			}

			/// <summary>
			/// 判断这个正则表达式是否正确，如果长度为0算false
			/// </summary>
			public static bool 是正确表达式(string 表达式)
			{
				if (类型.为空(表达式))
				{
					return false;
				}
				try
				{
					bool flag = Regex.IsMatch("a", 表达式, Rule);
					return true;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					ProjectData.ClearProjectError();
				}
				return false;
			}

			/// <summary>
			/// 正则替换内容
			/// </summary>
			public static string 替换(string 文本, params string[] 表达式)
			{
				checked
				{
					int num = 表达式.Length - 1;
					if (num > 0)
					{
						if (数学.是偶数(num))
						{
							num--;
						}
						int num2 = num;
						for (int i = 0; i <= num2; i += 2)
						{
							if (包含(文本, 表达式[i]))
							{
								文本 = Regex.Replace(文本标准化(ref 文本), FixRN(ref 表达式[i]), FixRN(ref 表达式[i + 1]), Rule);
							}
							if (类型.为空(文本))
							{
								break;
							}
						}
					}
					return 文本;
				}
			}

			/// <summary>
			/// 把文本中符合表达式的部分都去除
			/// </summary>
			public static string 去除(string 文本, params string[] 表达式)
			{
				for (int i = 0; i < 表达式.Length; i = checked(i + 1))
				{
					string p = 表达式[i];
					if (文本.Length > 0 && 是正确表达式(p))
					{
						文本 = Regex.Replace(文本标准化(ref 文本), FixRN(ref p), "", Rule);
					}
				}
				return 文本;
			}

			/// <summary>
			/// 文本当中是否可以匹配到表达式当中的一个
			/// </summary>
			public static bool 包含(string 文本, params string[] 表达式)
			{
				if (类型.为空(文本))
				{
					return false;
				}
				for (int i = 0; i < 表达式.Length; i = checked(i + 1))
				{
					string p = 表达式[i];
					if (是正确表达式(p) && Regex.IsMatch(文本标准化(ref 文本), FixRN(ref p), Rule))
					{
						return true;
					}
				}
				return false;
			}

			/// <summary>
			/// 相当于 Regex.Match ，返回的是字符串，
			/// </summary>
			public static string 检索第一个(string 文本, string 表达式)
			{
				if (类型.为空(文本) || !是正确表达式(表达式))
				{
					return "";
				}
				return Regex.Match(文本标准化(ref 文本), FixRN(ref 表达式), Rule).ToString();
			}

			/// <summary>
			/// 相当于 Regex.Matches
			/// </summary>
			public static List<Match> 检索(string 文本, string 表达式)
			{
				List<Match> list = new List<Match>();
				if (类型.为空(文本) || !是正确表达式(表达式))
				{
					return list;
				}
				IEnumerator enumerator = default(IEnumerator);
				try
				{
					enumerator = Regex.Matches(文本标准化(ref 文本), FixRN(ref 表达式), Rule).GetEnumerator();
					while (enumerator.MoveNext())
					{
						Match item = (Match)enumerator.Current;
						list.Add(item);
					}
				}
				finally
				{
					if (enumerator is IDisposable)
					{
						(enumerator as IDisposable).Dispose();
					}
				}
				return list;
			}

			/// <summary>
			/// 检索后直接返回对应序号个的内容，序号从0开始
			/// </summary>
			public static string 检索(string 文本, string 表达式, uint 序号)
			{
				List<Match> list = 检索(文本, 表达式);
				if (序号 >= list.Count)
				{
					return "";
				}
				return list[checked((int)序号)].ToString();
			}

			/// <summary>
			/// 把文本进行分块，不匹配表达式的一块，匹配表达式的一块，组成一个列表
			/// </summary>
			public static List<string> 分块(string 文本, string 表达式)
			{
				List<Match> list = 检索(文本, 表达式);
				List<string> list2 = new List<string>();
				checked
				{
					if (list.Count < 1)
					{
						list2.Add(文本);
					}
					else
					{
						uint num = 0u;
						int num2 = 0;
						foreach (Match item in list)
						{
							num2 = (int)(unchecked((long)item.Index) - unchecked((long)num));
							if (num2 != 0)
							{
								list2.Add(文本.Substring((int)num, num2));
								num = (uint)(unchecked((long)num) + unchecked((long)num2));
							}
							list2.Add(item.ToString());
							num = (uint)(unchecked((long)num) + unchecked((long)item.Length));
						}
						if (num != 文本.Length - 1)
						{
							list2.Add(文本.Substring((int)num));
						}
					}
					return list2;
				}
			}

			/// <summary>
			/// 把匹配到的表达式进行处理后，替换回文本当中
			/// </summary>
			public static string 高级替换(string 文本, string 表达式, Func<string, string> 处理)
			{
				if (类型.为空(文本) || !是正确表达式(表达式))
				{
					return 文本;
				}
				foreach (Match item in 检索(文本, 表达式))
				{
					string text = item.ToString();
					文本 = 文本.替换(文本, text, 处理(text));
				}
				return 文本;
			}

			/// <summary>
			/// 把文本分块，然后匹配的进行一个处理，非匹配的进行另外一个处理
			/// </summary>
			public static string 高级分块(string 文本, string 表达式, Func<string, string> 匹配处理 = null, Func<string, string> 非匹配处理 = null)
			{
				if (类型.为空(文本) || !是正确表达式(表达式))
				{
					return 文本;
				}
				string text = "";
				bool flag = false;
				foreach (string item in 分块(文本, 表达式))
				{
					string text2 = item;
					if (包含(text2, 表达式))
					{
						if (类型.非空(匹配处理))
						{
							text2 = 匹配处理(text2);
						}
					}
					else if (类型.非空(非匹配处理))
					{
						text2 = 非匹配处理(text2);
					}
					text += text2;
				}
				return text;
			}
		}

		/// <summary>
		/// 戈登走過去的加密法
		/// </summary>
		public sealed class 走過去加密
		{
			private string key;

			private string tb;

			private int salt;

			private string fl;

			/// <summary>
			/// 获取或设置密钥
			/// </summary>
			public string 密钥
			{
				get
				{
					return key;
				}
				set
				{
					fl = "一丁七丈三上下不丑且世丘丙业丛丝丢两丧个中串丹为主丽之乌乎乐乖乘乙九也乳予争事二云互五井亚些亡交亦京亭亮亲人亿什仁仅仆仇介仍从仓仔他仗付仙令仪们仰价任份仿企伍伏伐休众优伙伞伟传伤伪伯估伴伶伸似但位低住体何余佛作你佣佩佳使侄例侍供依侦侧侨侮侵便促俊俗俘保信俩俭修俯俱倍倒倘候倚借倡倦债倾假偏停健偶偷偿傅傍储催傲傻像僚僵僻儿元兄兆先光克兔兢入全八公六共兵其具典兼兽内册冒军冠冤冬冰冲决况冶冷冻净凉减凑凝几凡凭凳凶凸凹出函凿刀刃分切刊刑划列则刚创初删判利别刮到制刷券刺刻剂剃削前剑剖剥剧剩剪副割劈力功加劣助努劫励劲劳势勇勉勒勤勺匀包匕化北匙匠匪医十午半协卑卒单南博卜占卡卧印即却卵卷卸卿厂厅厕厘厚原厦厨去县又叉及友双反叔取受叙叛口古句叨只叫召叮可台史右叶号叼吃各吉名后吏吐向吓吕吗君吞否吧吨吩含听启吴吵吸吹吼呀呆呈告员呜呢味呼命和咏咐咬咱咳咸咽哀品哄哈响哑哗哥哨哪哭哲唇唉唐唤唯唱啄商啊啦喂喇喉喊喘喝喷嗓嗽嘉嘱嘴器嚣嚷嚼囊囚四回因园困囱围固圆圈土圣在地场圾址均坊坏坐坑块坚坝坟坡坦垂垃垄型垒垦垫垮埋城域培基堂堆堡堤堪堵塌塑塔塘塞填境墓墙增墨壁士壮壶壹复夏夕多夜够大天夫夭夯夷夸夹奇奉奏奔奖套奠奥女奴奶奸她好如妄妈妖妙妥妨妹妻始姐姑姓委姜姥姨姻姿威娃娄娇娘娱婆婚婶嫁嫂嫌嫩子孔孕字孙孝孟季孤孩宁它宅宇守安宋完宏宗官宙定宜宝审客宣室宦宪宫宰害宴宵家容宽宾宿寄密寇富寒察寡寨寸寺寻封射将尉尊小尖尘尚尤尸尺尼尾尿局居屈届屋屑展屠屡屯山屿岔岗岛岩岭岳岸峡峰崇崖崭川州巡巢工左巧巨巩巫己已巴巾布帆希帐帖帘帚帜帝带席帮常帽幅幕干年并幸幺幻幼幽广床序库底店府废度座庭康廉廊延建开弃弄弊弓引弟张弦弯弹强录形彩彪彭影役彻彼往征径待很律徐徒得御循微德心必忆忌忍志忘忙忠忧快念忽态怎怒怕怖怜思怠急性怨怪恋恐恒恢恨恩恭息恰恳恶恼悄悉悔悟悠患悦您悬悲悼情惊惑惕惜惠惧惨惩惭惯惰想惹愁愈愉意愚感愤愧愿慈慌慎慕慢慧慨慰懂懒戈成我戒或战戚截户房所扁扇手才扎扑扒扔托扛扣扩扬扭扮扯扰扶批找承技抄把抓投抖抗折抚抛抢护报披抬抱抵抹押抽担拆拉拌拍拐拒拔拘招拜拢拣拥拦拨择括拳拴拼拾拿持挂指按挎挖挠挡挣挤挥挨挪振挺挽捆捉捎捏捐捕捞损捡换捧据捷掀授掉掌掏排掘掠探接控推掩掰揉描提插握揪揭援搁搂搅搏搜搞搬搭携摄摆摇摊摔摘摧摩摸撇撑撒撕撞撤播操擦攀支收改攻放政故效敌敏救教敞散数敲整文斑斗料斜斤斥斧斩斯新方施旁旅旋族旗既日旦旨旬旭旱时旷旺昂昆昌明昏易昔星映春昨是昼晃晌晒晓晕晚晨景晴晶智暂暑暖暗暮暴曲更最月有朋服朗望朝期木未末本术朱朴朵机朽杀杆李杏材村杜束杠来杨杯松板极构析枕林果枝枣枪枯架柄柏某染柔柜查柱柳柴柿栋栏栗校株样核根格栽桂桃框案桌桐桑档桥桨桶梁梅梢梨梯械梳检棉棋棍棒棕棘棚森棵椅植椒楼概榆榜榨榴槐槽模横樱橘橡欠次欣欧欲欺歇歉歌止正此步武歪歹死歼殃殊残殖段殷殿毁母比毙毛毫毯民气氧水永汁求汗江池污汤汪汽沃沉沙沟没沫河沸油治沾沿泄泉泊泛泡波泥注泪泰泳泻泼泽洁洋洒洗洞津洪洮洲活洽派流浅浆浇浊测济浑浓浙浩浪浮浴海浸涂消涉涌涛涝润涨涩液淋淘淡深混淹添清渐渔渗渠渡渣温港渴游湖湾湿溉源溜溪滋滑滔滚满滤滥滨滩滴漂漆漏演漠漫潜潮澡激灌火灭灯灰灵灶灾灿炉炊炎炒炕炭炮炸炼烂烈烘烛烟烤烦烧烫热焦焰然煌煎煤照煮熄熊熔熟燃燕燥爆爪爬爵父爷爸爹爽版牌牙牛牢牧物牲牵特牺犁犬犯状犹狂狐狗狠狡狭狮狱狸狼猎猛猜猪猫献猴猾率玉王玩环现玻珍珠班球理琴瑞璃瓜瓣瓦瓶甘甜生用田甲申电男畅界畏留略番疆疏疗疤疫疮疯疲疼疾病症痒痕痛痰瘦登白百的皆皇皮皱皿盆盈益盏盐监盒盗盘盛盟目盯盲直相盼盾省眉看真眠眨眯眼着睁睛睡督睬瞎瞒瞧矗矛矢知矩石矿码砌砍研砖破础硬确碌碎碑碗碧碰磁磨示礼社祖祝神祟祥祭祸禁福禽禾秀私秃秆秉秋种科秒秘租秤秧秩积称移稀程稍税稠稳稻稼稿穆穗穴究空穿突窃窄窑窗窜窝立站章竭端竹竿笋笑笔笛符笨第笼等筋筐筑筒答策筛筝筹签简算管箩箭箱篇篮籍米粉粒粗粘粟粤粥粮粱精糊糕糖糟糠系素索紫累絮繁纠红纤约级纪纯纱纲纳纵纷纸纹纺纽线练组细织终绍经绑绒结绕绘给络绝绞统绢绣绩绪续绳维绵绸绿缎缓编缘缝缠缩缴缸缺罐网罕罚罢罩罪置羊美羔羞羡群羹羽翁翅翠翻翼耀老考者而耍耐耕耗耳耽聂聋职聘聚聪肉肌肚肝肠股肢肤肥肩肯育肺肾肿胀胁胃胆背胖胜胞胡胳胶胸能脂脆脉脊脏脑脖脚脱脸脾腊腐腔腥腰腹腾腿膀膊膏膛膜膝膨臂臣自臭至致臼舀舅舌舍舒舞舟航般舰舱船艇艘良艳艺节芒芝芦芬花芳芹芽苍苗若苦英苹茂范茄茅茎茧茫茶草荒荡荣药荷莫莲获莽菊菌菜菠萄萌萍萝营落葛葡董葬葱葵蒙蒜蒸蓄蓝蓬蔑蔬蔽蕉薄薪薯藏虎虏虐虑虫虹虾蚀蚁蚂蚊蚕蛇蛙蛛蛮蛾蜀蜂蜓蜘蜜蜡蜻蝇蝴蝶融螺蠢血行衔街衣补表衫衬衰袄袋袍袖袜被袭裁裂装裕裙裤裳裹西要覆见规视览觉角解言誓警计订认讨让训议讯记讲许论讽设访证评识诉诊词译试诗诚话诞询该详语误诱说诵请诸读课谁调谅谈谊谋谎谜谢谣谦谨谱谷豆象豪貌贝贞负贡财责贤败货质贩贪贫购贯贱贴贵贷贸费贺贼贿资赌赏赔赖赚赛赞赠赢赤赫走赴赶起趁超越趋趟趣足趴跃跌跑距跟跨跪路跳践踏踢踩踪蹄蹈蹦蹲躁身躬躲躺车轧转轮软轰轻载轿较辅辆辈辉输辛辜辞辟辣辨辩辫辰辱辽达迁迅迎运近返进远违连迟迫述迷迹追退适逃逆选透逐递途逗通逝速造逢逮逸逼遇遍道遗遣遥遭遮遵避邀邑那邪邮邻郊郎部都鄙配酒酬酱酷酸酿醉醋醒采释里重量金针钉钓钟钢钥钩钱钳钻铁铃铅铜铲银铸铺链销锁锄锅锈锋锐错锡锣锤锦键锯锹锻镇镜镰长门闪闭问闯闰闲间闷闸闹闻阀阁阅阔队防阳阴阵阶阻阿附降限陕陡院除险陪陵陶陷隆随隐隔隙障隶难雀雁雄雅集雕雨雪零雷雹雾震霉霍霜霞露霸青静靠面革鞋鞠鞭韭音韵页顶顷项顺须顽顾顿颂预领颈频颗题颜额颠颤飘食餐饥饭饮饰饱饲饶饺饼饿馅馆馋馒首香马驮驰驱驳驴驶驻驼驾骂骄骆验骑骗骡骤骨高鬼魂魄魔鱼鲜鸟鸣鸦鸭鸽鹅鹊鹰鹿麦麻黄黍黑默鼎鼓鼠鼻齐齿龄龙龟";
					value = 去重(左(筛选字符(value, fl), 30u));
					while (value.Length < 5)
					{
						value += 数学.随机.当中字符(fl);
					}
					key = 左(value, 30u);
					salt = 0;
					tb = "";
					生成字表();
				}
			}

			/// <summary>
			/// 新建一个加解密，并使用指定的密钥，如果密钥不正确则会自动生成一个，密钥应该是5-30个简体汉字
			/// </summary>
			public 走過去加密(string 密钥)
			{
				this.密钥 = 密钥;
			}

			/// <summary>
			/// 新建一个加解密，并自动生成一个密钥
			/// </summary>
			public 走過去加密()
			{
				密钥 = "";
			}

			private void 生成字表()
			{
				string text = "";
				int num = 1;
				string text2 = fl + 左(fl, 300u);
				string 密钥 = this.密钥;
				string text3 = 密钥;
				checked
				{
					foreach (char c in text3)
					{
						num = ("一丁七丈三上下不丑且世丘丙业丛丝丢两丧个中串丹为主丽之乌乎乐乖乘乙九也乳予争事二云互五井亚些亡交亦京亭亮亲人亿什仁仅仆仇介仍从仓仔他仗付仙令仪们仰价任份仿企伍伏伐休众优伙伞伟传伤伪伯估伴伶伸似但位低住体何余佛作你佣佩佳使侄例侍供依侦侧侨侮侵便促俊俗俘保信俩俭修俯俱倍倒倘候倚借倡倦债倾假偏停健偶偷偿傅傍储催傲傻像僚僵僻儿元兄兆先光克兔兢入全八公六共兵其具典兼兽内册冒军冠冤冬冰冲决况冶冷冻净凉减凑凝几凡凭凳凶凸凹出函凿刀刃分切刊刑划列则刚创初删判利别刮到制刷券刺刻剂剃削前剑剖剥剧剩剪副割劈力功加劣助努劫励劲劳势勇勉勒勤勺匀包匕化北匙匠匪医十午半协卑卒单南博卜占卡卧印即却卵卷卸卿厂厅厕厘厚原厦厨去县又叉及友双反叔取受叙叛口古句叨只叫召叮可台史右叶号叼吃各吉名后吏吐向吓吕吗君吞否吧吨吩含听启吴吵吸吹吼呀呆呈告员呜呢味呼命和咏咐咬咱咳咸咽哀品哄哈响哑哗哥哨哪哭哲唇唉唐唤唯唱啄商啊啦喂喇喉喊喘喝喷嗓嗽嘉嘱嘴器嚣嚷嚼囊囚四回因园困囱围固圆圈土圣在地场圾址均坊坏坐坑块坚坝坟坡坦垂垃垄型垒垦垫垮埋城域培基堂堆堡堤堪堵塌塑塔塘塞填境墓墙增墨壁士壮壶壹复夏夕多夜够大天夫夭夯夷夸夹奇奉奏奔奖套奠奥女奴奶奸她好如妄妈妖妙妥妨妹妻始姐姑姓委姜姥姨姻姿威娃娄娇娘娱婆婚婶嫁嫂嫌嫩子孔孕字孙孝孟季孤孩宁它宅宇守安宋完宏宗官宙定宜宝审客宣室宦宪宫宰害宴宵家容宽宾宿寄密寇富寒察寡寨寸寺寻封射将尉尊小尖尘尚尤尸尺尼尾尿局居屈届屋屑展屠屡屯山屿岔岗岛岩岭岳岸峡峰崇崖崭川州巡巢工左巧巨巩巫己已巴巾布帆希帐帖帘帚帜帝带席帮常帽幅幕干年并幸幺幻幼幽广床序库底店府废度座庭康廉廊延建开弃弄弊弓引弟张弦弯弹强录形彩彪彭影役彻彼往征径待很律徐徒得御循微德心必忆忌忍志忘忙忠忧快念忽态怎怒怕怖怜思怠急性怨怪恋恐恒恢恨恩恭息恰恳恶恼悄悉悔悟悠患悦您悬悲悼情惊惑惕惜惠惧惨惩惭惯惰想惹愁愈愉意愚感愤愧愿慈慌慎慕慢慧慨慰懂懒戈成我戒或战戚截户房所扁扇手才扎扑扒扔托扛扣扩扬扭扮扯扰扶批找承技抄把抓投抖抗折抚抛抢护报披抬抱抵抹押抽担拆拉拌拍拐拒拔拘招拜拢拣拥拦拨择括拳拴拼拾拿持挂指按挎挖挠挡挣挤挥挨挪振挺挽捆捉捎捏捐捕捞损捡换捧据捷掀授掉掌掏排掘掠探接控推掩掰揉描提插握揪揭援搁搂搅搏搜搞搬搭携摄摆摇摊摔摘摧摩摸撇撑撒撕撞撤播操擦攀支收改攻放政故效敌敏救教敞散数敲整文斑斗料斜斤斥斧斩斯新方施旁旅旋族旗既日旦旨旬旭旱时旷旺昂昆昌明昏易昔星映春昨是昼晃晌晒晓晕晚晨景晴晶智暂暑暖暗暮暴曲更最月有朋服朗望朝期木未末本术朱朴朵机朽杀杆李杏材村杜束杠来杨杯松板极构析枕林果枝枣枪枯架柄柏某染柔柜查柱柳柴柿栋栏栗校株样核根格栽桂桃框案桌桐桑档桥桨桶梁梅梢梨梯械梳检棉棋棍棒棕棘棚森棵椅植椒楼概榆榜榨榴槐槽模横樱橘橡欠次欣欧欲欺歇歉歌止正此步武歪歹死歼殃殊残殖段殷殿毁母比毙毛毫毯民气氧水永汁求汗江池污汤汪汽沃沉沙沟没沫河沸油治沾沿泄泉泊泛泡波泥注泪泰泳泻泼泽洁洋洒洗洞津洪洮洲活洽派流浅浆浇浊测济浑浓浙浩浪浮浴海浸涂消涉涌涛涝润涨涩液淋淘淡深混淹添清渐渔渗渠渡渣温港渴游湖湾湿溉源溜溪滋滑滔滚满滤滥滨滩滴漂漆漏演漠漫潜潮澡激灌火灭灯灰灵灶灾灿炉炊炎炒炕炭炮炸炼烂烈烘烛烟烤烦烧烫热焦焰然煌煎煤照煮熄熊熔熟燃燕燥爆爪爬爵父爷爸爹爽版牌牙牛牢牧物牲牵特牺犁犬犯状犹狂狐狗狠狡狭狮狱狸狼猎猛猜猪猫献猴猾率玉王玩环现玻珍珠班球理琴瑞璃瓜瓣瓦瓶甘甜生用田甲申电男畅界畏留略番疆疏疗疤疫疮疯疲疼疾病症痒痕痛痰瘦登白百的皆皇皮皱皿盆盈益盏盐监盒盗盘盛盟目盯盲直相盼盾省眉看真眠眨眯眼着睁睛睡督睬瞎瞒瞧矗矛矢知矩石矿码砌砍研砖破础硬确碌碎碑碗碧碰磁磨示礼社祖祝神祟祥祭祸禁福禽禾秀私秃秆秉秋种科秒秘租秤秧秩积称移稀程稍税稠稳稻稼稿穆穗穴究空穿突窃窄窑窗窜窝立站章竭端竹竿笋笑笔笛符笨第笼等筋筐筑筒答策筛筝筹签简算管箩箭箱篇篮籍米粉粒粗粘粟粤粥粮粱精糊糕糖糟糠系素索紫累絮繁纠红纤约级纪纯纱纲纳纵纷纸纹纺纽线练组细织终绍经绑绒结绕绘给络绝绞统绢绣绩绪续绳维绵绸绿缎缓编缘缝缠缩缴缸缺罐网罕罚罢罩罪置羊美羔羞羡群羹羽翁翅翠翻翼耀老考者而耍耐耕耗耳耽聂聋职聘聚聪肉肌肚肝肠股肢肤肥肩肯育肺肾肿胀胁胃胆背胖胜胞胡胳胶胸能脂脆脉脊脏脑脖脚脱脸脾腊腐腔腥腰腹腾腿膀膊膏膛膜膝膨臂臣自臭至致臼舀舅舌舍舒舞舟航般舰舱船艇艘良艳艺节芒芝芦芬花芳芹芽苍苗若苦英苹茂范茄茅茎茧茫茶草荒荡荣药荷莫莲获莽菊菌菜菠萄萌萍萝营落葛葡董葬葱葵蒙蒜蒸蓄蓝蓬蔑蔬蔽蕉薄薪薯藏虎虏虐虑虫虹虾蚀蚁蚂蚊蚕蛇蛙蛛蛮蛾蜀蜂蜓蜘蜜蜡蜻蝇蝴蝶融螺蠢血行衔街衣补表衫衬衰袄袋袍袖袜被袭裁裂装裕裙裤裳裹西要覆见规视览觉角解言誓警计订认讨让训议讯记讲许论讽设访证评识诉诊词译试诗诚话诞询该详语误诱说诵请诸读课谁调谅谈谊谋谎谜谢谣谦谨谱谷豆象豪貌贝贞负贡财责贤败货质贩贪贫购贯贱贴贵贷贸费贺贼贿资赌赏赔赖赚赛赞赠赢赤赫走赴赶起趁超越趋趟趣足趴跃跌跑距跟跨跪路跳践踏踢踩踪蹄蹈蹦蹲躁身躬躲躺车轧转轮软轰轻载轿较辅辆辈辉输辛辜辞辟辣辨辩辫辰辱辽达迁迅迎运近返进远违连迟迫述迷迹追退适逃逆选透逐递途逗通逝速造逢逮逸逼遇遍道遗遣遥遭遮遵避邀邑那邪邮邻郊郎部都鄙配酒酬酱酷酸酿醉醋醒采释里重量金针钉钓钟钢钥钩钱钳钻铁铃铅铜铲银铸铺链销锁锄锅锈锋锐错锡锣锤锦键锯锹锻镇镜镰长门闪闭问闯闰闲间闷闸闹闻阀阁阅阔队防阳阴阵阶阻阿附降限陕陡院除险陪陵陶陷隆随隐隔隙障隶难雀雁雄雅集雕雨雪零雷雹雾震霉霍霜霞露霸青静靠面革鞋鞠鞭韭音韵页顶顷项顺须顽顾顿颂预领颈频颗题颜额颠颤飘食餐饥饭饮饰饱饲饶饺饼饿馅馆馋馒首香马驮驰驱驳驴驶驻驼驾骂骄骆验骑骗骡骤骨高鬼魂魄魔鱼鲜鸟鸣鸦鸭鸽鹅鹊鹰鹿麦麻黄黍黑默鼎鼓鼠鼻齐齿龄龙龟".Contains(Conversions.ToString(c)) ? 7 : ((!"一丁七丈三上下不醜且世丘丙業叢絲丟兩喪個中串丹為主麗之烏乎樂乖乘乙九也乳予爭事二雲互五井亞些亡交亦京亭亮親人億什仁僅僕仇介仍從倉仔他仗付仙令儀們仰價任份仿企伍伏伐休眾優夥傘偉傳傷偽伯估伴伶伸似但比特低住體何餘佛作你傭佩佳使侄例侍供依偵側僑侮侵便促俊俗俘保信倆儉修俯俱倍倒倘候倚借倡倦債傾假偏停健偶偷償傅傍儲催傲傻像僚僵僻兒元兄兆先光克兔兢入全八公六共兵其具典兼獸內册冒軍冠冤冬冰沖決况冶冷凍淨凉减凑凝幾凡憑凳凶凸凹出函鑿刀刃分切刊刑劃列則剛創初删判利別刮到制刷券刺刻劑剃削前劍剖剝劇剩剪副割劈力功加劣助努劫勵勁勞勢勇勉勒勤勺勻包匕化北匙匠匪醫十午半協卑卒單南博蔔占卡臥印即卻卵卷卸卿廠廳廁厘厚原厦廚去縣又叉及友雙反叔取受敘叛口古句叨只叫召叮可臺史右葉號叼吃各吉名後吏吐向嚇呂嗎君吞否吧噸吩含聽啟吳吵吸吹吼呀呆呈告員嗚呢味呼命和咏咐咬咱咳鹹咽哀品哄哈響啞嘩哥哨哪哭哲唇唉唐喚唯唱啄商啊啦喂喇喉喊喘喝噴嗓嗽嘉囑嘴器囂嚷嚼囊囚四回因園困囪圍固圓圈土聖在地場圾址均坊壞坐坑塊堅壩墳坡坦垂垃壟型壘墾墊垮埋城域培基堂堆堡堤堪堵塌塑塔塘塞填境墓牆增墨壁士壯壺壹複夏夕多夜够大天夫夭夯夷誇夾奇奉奏奔獎套奠奧女奴奶奸她好如妄媽妖妙妥妨妹妻始姐姑姓委薑姥姨姻姿威娃婁嬌娘娛婆婚嬸嫁嫂嫌嫩子孔孕字孫孝孟季孤孩寧它宅宇守安宋完宏宗官宙定宜寶審客宣室宦憲宮宰害宴宵家容寬賓宿寄密寇富寒察寡寨寸寺尋封射將尉尊小尖塵尚尤屍尺尼尾尿局居屈届屋屑展屠屢屯山嶼岔崗島岩嶺嶽岸峽峰崇崖嶄川州巡巢工左巧巨鞏巫己已巴巾布帆希帳帖簾帚幟帝帶席幫常帽幅幕幹年並幸么幻幼幽廣床序庫底店府廢度座庭康廉廊延建開弃弄弊弓引弟張弦彎彈强錄形彩彪彭影役徹彼往征徑待很律徐徒得禦循微德心必憶忌忍志忘忙忠憂快念忽態怎怒怕怖憐思怠急性怨怪戀恐恒恢恨恩恭息恰懇惡惱悄悉悔悟悠患悅您懸悲悼情驚惑惕惜惠懼慘懲慚慣惰想惹愁愈愉意愚感憤愧願慈慌慎慕慢慧慨慰懂懶戈成我戒或戰戚截戶房所扁扇手才紮撲扒扔托扛扣擴揚扭扮扯擾扶批找承技抄把抓投抖抗折撫拋搶護報披抬抱抵抹押抽擔拆拉拌拍拐拒拔拘招拜攏揀擁攔撥擇括拳拴拼拾拿持掛指按挎挖撓擋掙擠揮挨挪振挺挽捆捉捎捏捐捕撈損撿換捧據捷掀授掉掌掏排掘掠探接控推掩掰揉描提插握揪揭援擱摟攪搏搜搞搬搭攜攝擺搖攤摔摘摧摩摸撇撐撒撕撞撤播操擦攀支收改攻放政故效敵敏救教敞散數敲整文斑鬥料斜斤斥斧斬斯新方施旁旅旋族旗既日旦旨旬旭旱時曠旺昂昆昌明昏易昔星映春昨是晝晃晌曬曉暈晚晨景晴晶智暫暑暖暗暮暴曲更最月有朋服朗望朝期木未末本術朱樸朵機朽殺杆李杏材村杜束杠來楊杯松板極構析枕林果枝棗槍枯架柄柏某染柔櫃查柱柳柴柿棟欄栗校株樣核根格栽桂桃框案桌桐桑檔橋槳桶梁梅梢梨梯械梳檢棉棋棍棒棕棘棚森棵椅植椒樓概榆榜榨榴槐槽模橫櫻橘橡欠次欣歐欲欺歇歉歌止正此步武歪歹死殲殃殊殘殖段殷殿毀母斃毛毫毯民氣氧水永汁求汗江池污湯汪汽沃沉沙溝沒沫河沸油治沾沿泄泉泊泛泡波泥注淚泰泳瀉潑澤潔洋灑洗洞津洪洮洲活洽派流淺漿澆濁測濟渾濃浙浩浪浮浴海浸塗消涉湧濤澇潤漲澀液淋淘淡深混淹添清漸漁滲渠渡渣溫港渴遊湖灣濕溉源溜溪滋滑滔滾滿濾濫濱灘滴漂漆漏演漠漫潜潮澡激灌火滅燈灰靈灶灾燦爐炊炎炒炕炭炮炸煉爛烈烘燭烟烤煩燒燙熱焦焰然煌煎煤照煮熄熊熔熟燃燕燥爆爪爬爵父爺爸爹爽版牌牙牛牢牧物牲牽犧犁犬犯狀猶狂狐狗狠狡狹獅獄狸狼獵猛猜猪猫獻猴猾率玉王玩環現玻珍珠班球理琴瑞璃瓜瓣瓦瓶甘甜生用田甲申電男暢界畏留略番疆疏療疤疫瘡瘋疲疼疾病症癢痕痛痰瘦登白百的皆皇皮皺皿盆盈益盞鹽監盒盜盤盛盟目盯盲直相盼盾省眉看真眠眨眯眼著睜睛睡督睬瞎瞞瞧矗矛矢知矩石礦碼砌砍研磚破礎硬確碌碎碑碗碧碰磁磨示禮社祖祝神祟祥祭禍禁福禽禾秀私禿稈秉秋種科秒秘租秤秧秩積稱移稀程稍稅稠穩稻稼稿穆穗穴究空穿突竊窄窑窗竄窩立站章竭端竹竿笋笑筆笛符笨第籠等筋筐築筒答策篩箏籌簽簡算管籮箭箱篇籃籍米粉粒粗粘粟粵粥糧粱精糊糕糖糟糠系素索紫累絮繁糾紅纖約級紀純紗綱納縱紛紙紋紡紐線練組細織終紹經綁絨結繞繪給絡絕絞統絹繡績緒續繩維綿綢綠緞緩編緣縫纏縮繳缸缺罐網罕罰罷罩罪置羊美羔羞羨群羹羽翁翅翠翻翼耀老考者而耍耐耕耗耳耽聶聾職聘聚聰肉肌肚肝腸股肢膚肥肩肯育肺腎腫脹脅胃膽背胖勝胞胡胳膠胸能脂脆脈脊髒腦脖脚脫臉脾臘腐腔腥腰腹騰腿膀膊膏膛膜膝膨臂臣自臭至致臼舀舅舌舍舒舞舟航般艦艙船艇艘良豔藝節芒芝蘆芬花芳芹芽蒼苗若苦英蘋茂範茄茅莖繭茫茶草荒蕩榮藥荷莫蓮獲莽菊菌菜菠萄萌萍蘿營落葛葡董葬葱葵蒙蒜蒸蓄藍蓬蔑蔬蔽蕉薄薪薯藏虎虜虐慮蟲虹蝦蝕蟻螞蚊蠶蛇蛙蛛蠻蛾蜀蜂蜓蜘蜜蠟蜻蠅蝴蝶融螺蠢血行銜街衣補錶衫襯衰襖袋袍袖襪被襲裁裂裝裕裙褲裳裹西要覆見規視覽覺角解言誓警計訂認討讓訓議訊記講許論諷設訪證評識訴診詞譯試詩誠話誕詢該詳語誤誘說誦請諸讀課誰調諒談誼謀謊謎謝謠謙謹譜穀豆象豪貌貝貞負貢財責賢敗貨質販貪貧購貫賤貼貴貸貿費賀賊賄資賭賞賠賴賺賽贊贈贏赤赫走赴趕起趁超越趨趟趣足趴躍跌跑距跟跨跪路跳踐踏踢踩踪蹄蹈蹦蹲躁身躬躲躺車軋轉輪軟轟輕載轎較輔輛輩輝輸辛辜辭辟辣辨辯辮辰辱遼達遷迅迎運近返進遠違連遲迫述迷迹追退適逃逆選透逐遞途逗通逝速造逢逮逸逼遇遍道遺遣遙遭遮遵避邀邑那邪郵鄰郊郎部都鄙配酒酬醬酷酸釀醉醋醒采釋裏重量金針釘釣鐘鋼鑰鉤錢鉗鑽鐵鈴鉛銅鏟銀鑄鋪鏈銷鎖鋤鍋鏽鋒銳錯錫鑼錘錦鍵鋸鍬鍛鎮鏡鐮長門閃閉問闖閏閑間悶閘鬧聞閥閣閱闊隊防陽陰陣階阻阿附降限陝陡院除險陪陵陶陷隆隨隱隔隙障隸難雀雁雄雅集雕雨雪零雷雹霧震黴霍霜霞露霸青靜靠面革鞋鞠鞭韭音韻頁頂頃項順須頑顧頓頌預領頸頻顆題顏額顛顫飄食餐饑飯飲飾飽飼饒餃餅餓餡館饞饅首香馬馱馳驅駁驢駛駐駝駕罵驕駱驗騎騙騾驟骨高鬼魂魄魔魚鮮鳥鳴鴉鴨鴿鹅鵲鷹鹿麥麻黃黍黑默鼎鼓鼠鼻齊齒齡龍龜".Contains(Conversions.ToString(c))) ? 13 : 19));
						string str = text2.Substring(fl.IndexOf(c), num);
						int num2 = c;
						if (数学.是偶数(num2))
						{
							text = Strings.StrReverse(text);
						}
						text += str;
						ref int reference = ref salt;
						reference = (int)Math.Round((double)reference + (double)num2 / 2.0);
					}
					salt = unchecked(salt % 6) + 4;
					text = 去重(text);
					int num3 = 255;
					if (text.Length < num3)
					{
						int num4 = fl.Length - num;
						for (int num2 = num4; num2 >= 1; num2 += -1)
						{
							char c = fl[num2];
							if (!text.Contains(Conversions.ToString(c)))
							{
								text += Conversions.ToString(c);
							}
							if (text.Length > num3)
							{
								break;
							}
						}
					}
					tb = 左(text, (uint)num3);
				}
			}

			/// <summary>
			/// 把字节数组进行加密
			/// </summary>
			public string 加密(byte[] 字节数组)
			{
				string text = "";
				checked
				{
					if (类型.非空(字节数组))
					{
						int num = 0;
						int num2 = salt;
						string text2 = "";
						foreach (byte index in 字节数组)
						{
							if (num > num2)
							{
								text2 = 数学.随机.当中字符(tb, (uint)salt);
								text += text2;
								num = 0;
								num2++;
							}
							text += Conversions.ToString(tb[index]);
							num++;
						}
					}
					return text;
				}
			}

			/// <summary>
			/// 对UTF8字符串进行加密
			/// </summary>
			public string 加密(string 文本)
			{
				return 加密(文本转字节数组(文本));
			}

			/// <summary>
			/// 对密文进行解密，输出为字节数组 
			/// </summary>
			public byte[] 解密为字节数组(string 密文)
			{
				if (类型.为空(密文))
				{
					return null;
				}
				List<byte> list = new List<byte>();
				int num = 0;
				int num2 = salt;
				密文 = 筛选字符(密文, tb);
				checked
				{
					int num3 = 密文.Length - 1;
					for (int i = 0; i <= num3; i++)
					{
						if (num > num2)
						{
							i += salt - 1;
							num2++;
							num = 0;
						}
						else
						{
							list.Add((byte)tb.IndexOf(密文[i]));
							num++;
						}
					}
					return list.ToArray();
				}
			}

			/// <summary>
			/// 解密密文到UTF8字符串
			/// </summary>
			public string 解密为字符串(string 密文)
			{
				return 字节数组转文本(解密为字节数组(密文));
			}
		}

		/// <summary>
		/// 对数据进行逐行登记用的
		/// </summary>
		public class 数据登记表
		{
			private string all;

			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string _连接符;

			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private bool _忽略空内容;

			/// <summary>
			/// 连接用的连接符
			/// </summary>
			public string 连接符
			{
				get;
			}

			/// <summary>
			/// 如果加入的内容是空的，那么是否还要加入
			/// </summary>
			public bool 忽略空内容
			{
				get;
			}

			/// <summary>
			/// 新建数据登记表，定义连接文本，默认为: 来连接
			/// </summary>
			public 数据登记表(string 连接符 = ": ", bool 忽略空内容 = true)
			{
				all = "";
				_连接符 = 连接符;
				_忽略空内容 = 忽略空内容;
			}

			/// <summary>
			/// 增加一行内容
			/// </summary>
			public void 增加(string 内容)
			{
				if (内容.Length >= 1 || !忽略空内容)
				{
					if (all.Length > 0)
					{
						all += "\r\n";
					}
					all += 内容;
				}
			}

			/// <summary>
			/// 增加一行内容，标题 + 连接符 + 内容
			/// </summary>
			public void 增加(string 标题, string 内容)
			{
				if (!忽略空内容 || (内容.Length >= 1 && 标题.Length >= 1))
				{
					if (all.Length > 0)
					{
						all += "\r\n";
					}
					ref string reference = ref all;
					reference = reference + 标题 + 连接符 + 内容;
				}
			}

			/// <summary>
			/// 输出采集的数据
			/// </summary>
			public override string ToString()
			{
				return all;
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<string, string> _0024I43_002D0;

			public static Func<string, string> _0024I43_002D1;

			public static Func<string, string> _0024I43_002D2;

			public static Func<string, string> _0024I45_002D0;

			public static Func<string, string> _0024I45_002D1;

			public static Func<string, string> _0024I45_002D2;

			public static Func<string, string> _0024I47_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			internal string _Lambda_0024__43_002D0(string m)
			{
				return 压缩HTML(m);
			}

			internal string _Lambda_0024__43_002D1(string m)
			{
				string text = "";
				if (!m.EndsWith("\r\n"))
				{
					m += "\r\n";
				}
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				foreach (string item in 分行(m + "\r\n"))
				{
					string text2 = item;
					if (类型.为空(text2.Trim()))
					{
						if (flag5)
						{
							flag5 = false;
							text += "</p>";
						}
						else
						{
							text += "<br>";
						}
						if (flag2)
						{
							flag2 = false;
							text += "</ol>";
						}
						if (flag3)
						{
							flag3 = false;
							text += "</ul>";
						}
						if (flag4)
						{
							flag4 = false;
							text += "</blockquote>";
						}
					}
					else
					{
						if (text2.StartsWith(">") && !flag4)
						{
							flag4 = true;
							text += "<blockquote>";
						}
						if (flag4)
						{
							if (text2.StartsWith(">"))
							{
								text2 = 正则.去除(text2, "^>+");
							}
							else
							{
								flag4 = false;
								text += "</blockquote>";
							}
						}
						if (flag2 && !正则.包含(text2.Trim(), "^[0-9]+\\. "))
						{
							flag2 = false;
							text += "</ol>";
						}
						if (flag3 && !正则.包含(text2.Trim(), "^- "))
						{
							flag3 = false;
							text += "</ul>";
						}
						if (正则.包含(text2, "^\\#+ "))
						{
							if (flag5)
							{
								flag5 = false;
								text += "</p>";
							}
							int length = 提取最之前(text2, " ").Length;
							if (length <= 6)
							{
								text2 = 正则.去除(text2, "^\\#+ ", " \\#+$").Trim();
								string text3 = "h" + length.ToString() + ">";
								text2 = "<" + text3 + MD粗体斜体(text2) + "</" + text3;
							}
						}
						else if (正则.包含(text2.Trim(), "^-{3,}$"))
						{
							if (flag5)
							{
								flag5 = false;
								text += "</p>";
							}
							text2 = "<hr>";
						}
						else if (正则.包含(text2.Trim(), "```.*") && !flag)
						{
							if (flag5)
							{
								flag5 = false;
								text += "</p>";
							}
							string text3 = 去左(text2.Trim(), 3u);
							if (text3.Length > 0)
							{
								text3 = " class='" + text3 + "'";
							}
							text2 = "<pre><code" + text3 + ">";
							flag = true;
						}
						else if (Operators.CompareString(text2.Trim(), "```", TextCompare: false) != 0 && flag)
						{
							text2 = 替换(text2, " ", "&nbsp;") + "<br>";
						}
						else if (Operators.CompareString(text2.Trim(), "```", TextCompare: false) == 0 && flag)
						{
							flag = false;
							text2 = "</code></pre>";
						}
						else if (正则.包含(text2.Trim(), "^[0-9]+\\. "))
						{
							if (flag5)
							{
								flag5 = false;
								text += "</p>";
							}
							if (!flag2)
							{
								flag2 = true;
								text += "<ol>";
							}
							text2 = "<li>" + MD粗体斜体(提取之后(text2.Trim(), " ")) + "</li>";
						}
						else if (正则.包含(text2.Trim(), "^- "))
						{
							if (flag5)
							{
								flag5 = false;
								text += "</p>";
							}
							if (!flag3)
							{
								flag3 = true;
								text += "<ul>";
							}
							text2 = "<li>" + MD粗体斜体(提取之后(text2.Trim(), " ")) + "</li>";
						}
						else
						{
							if (!flag5)
							{
								text += "<p>";
								flag5 = true;
							}
							if (text2.EndsWith("  "))
							{
								text2 = text2.Trim() + "<br>";
							}
							text2 = MD粗体斜体(text2);
						}
						text += text2;
					}
				}
				text = 正则.替换(text, "(<br>)*<hr>(<br>)*", "<hr>", "<br></code></pre>", "</code></pre>", "><br></", "></", "<br></", "</");
				if (!提取最之后(text, "<p>").Contains("</p>"))
				{
					text += "</p>";
				}
				return text;
			}

			internal string _Lambda_0024__43_002D2(string m)
			{
				string text = 提取最之前(m, ">") + ">";
				string text2 = "</code>";
				string 文本 = checked(去右(去左(m, (uint)text.Length), (uint)text2.Length));
				文本 = 替换(文本, "<br>", "\r\n", "<", "&lt;", ">", "&gt;", "\r\n", "<br>");
				return text + 文本 + text2;
			}

			internal string _Lambda_0024__45_002D0(string m)
			{
				string text = 去右(提取最之后(m, "/").ToLower(), 1u);
				string text2 = 提取最之前(去左(m, 1u), "<");
				text2 = ((Operators.CompareString(text, "script", TextCompare: false) == 0) ? 压缩JS(text2) : ((Operators.CompareString(text, "style", TextCompare: false) != 0) ? 去连续重复(text2, " ") : 压缩CSS(text2)));
				return ">" + text2 + "</" + text + ">";
			}

			internal string _Lambda_0024__45_002D1(string m)
			{
				return 左(m, 1u) + 压缩JS(去右(去左(m, 1u), 1u)) + 右(m, 1u);
			}

			internal string _Lambda_0024__45_002D2(string m)
			{
				return 去除依附空格(正则.替换(m, " +", " "), "<>");
			}

			internal string _Lambda_0024__47_002D0(string m)
			{
				return 去除依附空格(正则.替换(m, " +", " "), "=(){},;\"");
			}
		}

		private static UTF8Encoding _0024STATIC_0024无BOM的UTF8编码_002400128181_0024m;

		private static StaticLocalInitFlag _0024STATIC_0024无BOM的UTF8编码_002400128181_0024m_0024Init;

		/// <summary>
		/// 对文本进行标准化处理，包括修正CRLF，把多余的控制符替换为空格，把tab换成4个空格
		/// </summary>
		public static string 文本标准化(ref string 文本)
		{
			if (文本 == null)
			{
				文本 = "";
			}
			else if (文本.Length > 0)
			{
				try
				{
					if (!文本.IsNormalized())
					{
						文本 = 文本.Normalize();
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					程序.出错(ex2);
					ProjectData.ClearProjectError();
				}
				if (包含(文本, "\r", "\n"))
				{
					文本 = 替换(文本, "\r\n", "\n", "\r", "\n", "\n", "\r\n");
				}
				if (包含(文本, "\t"))
				{
					文本 = 替换(文本, "\t", Strings.Space(4));
				}
				int num = 0;
				do
				{
					if (num != 10 && num != 13)
					{
						文本 = 替换(文本, Conversions.ToString(Strings.ChrW(num)), " ");
					}
					num = checked(num + 1);
				}
				while (num <= 31);
			}
			return 文本;
		}

		/// <summary>
		/// 检查文本内是否包含要寻找的内容的当中一项
		/// </summary>
		public static bool 包含(string 文本, params string[] 内容)
		{
			if (类型.为空(文本))
			{
				return false;
			}
			foreach (string text in 内容)
			{
				if (text.Length > 0 && 文本.Contains(text))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 检查文本内是否包含要寻找的内容的每一项
		/// </summary>
		public static bool 包含全部(string 文本, params string[] 内容)
		{
			if (类型.为空(文本))
			{
				return false;
			}
			foreach (string text in 内容)
			{
				if (text.Length > 0 && !文本.Contains(text))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// 一一对应替换掉文本中的相关内容
		/// </summary>
		public static string 替换(string 文本, params string[] 内容)
		{
			checked
			{
				int num = 内容.Length - 1;
				if (num > 0)
				{
					if (数学.是偶数(num))
					{
						num--;
					}
					int num2 = num;
					for (int i = 0; i <= num2; i += 2)
					{
						if (内容[i].Length > 0 && 文本.Contains(内容[i]))
						{
							文本 = 文本.Replace(内容[i], 内容[i + 1]);
						}
						if (类型.为空(文本))
						{
							break;
						}
					}
				}
				return 文本;
			}
		}

		/// <summary>
		/// 返回文本左边指定长度的字符串
		/// </summary>
		public static string 左(string 文本, uint 长度)
		{
			if (类型.为空(文本) || (long)长度 < 1L)
			{
				return "";
			}
			return Strings.Left(文本, checked((int)长度));
		}

		/// <summary>
		/// 返回文本右边指定长度的字符串
		/// </summary>
		public static string 右(string 文本, uint 长度)
		{
			if (类型.为空(文本) || (long)长度 < 1L)
			{
				return "";
			}
			return Strings.Right(文本, checked((int)长度));
		}

		/// <summary>
		/// 返回文本去掉右边指定长度的字符串
		/// </summary>
		public static string 去右(string 文本, uint 长度)
		{
			if (类型.为空(文本) || 长度 >= 文本.Length)
			{
				return "";
			}
			checked
			{
				return 左(文本, (uint)(unchecked((long)文本.Length) - unchecked((long)长度)));
			}
		}

		/// <summary>
		/// 返回文本去掉左边指定长度的字符串
		/// </summary>
		public static string 去左(string 文本, uint 长度)
		{
			if (类型.为空(文本) || 长度 >= 文本.Length)
			{
				return "";
			}
			checked
			{
				return 右(文本, (uint)(unchecked((long)文本.Length) - unchecked((long)长度)));
			}
		}

		/// <summary>
		/// 检查文本是否以相关内容开头
		/// </summary>
		public static bool 头(string 文本, params string[] 内容)
		{
			if (类型.为空(文本))
			{
				return false;
			}
			foreach (string text in 内容)
			{
				if (text.Length > 0 && 文本.StartsWith(text))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 检查文本是否以相关内容结尾
		/// </summary>
		public static bool 尾(string 文本, params string[] 内容)
		{
			if (类型.为空(文本))
			{
				return false;
			}
			foreach (string text in 内容)
			{
				if (text.Length > 0 && 文本.EndsWith(text))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 从前向后寻找，提取需要寻找的全部字符串之后的文本，如果不存在要寻找的就返回空字符串
		/// </summary>
		public static string 提取之后(string 文本, params string[] 寻找)
		{
			if (类型.为空(文本) || 类型.为空(寻找))
			{
				return "";
			}
			foreach (string text in 寻找)
			{
				if (text.Length > 0)
				{
					int num = Strings.InStr(文本, text);
					if (num < 1)
					{
						return "";
					}
					文本 = 文本.Substring(checked(num + text.Length - 1));
					continue;
				}
				return "";
			}
			return 文本;
		}

		/// <summary>
		/// 从后向前寻找，提取需要寻找的全部字符串之前的文本，如果不存在要寻找的就返回空字符串
		/// </summary>
		public static string 提取之前(string 文本, params string[] 寻找)
		{
			if (类型.为空(文本) || 类型.为空(寻找))
			{
				return "";
			}
			foreach (string text in 寻找)
			{
				if (text.Length > 0)
				{
					int num = Strings.InStrRev(文本, text);
					if (num < 1)
					{
						return "";
					}
					文本 = Strings.Left(文本, checked(num - 1));
					continue;
				}
				return "";
			}
			return 文本;
		}

		/// <summary>
		/// 从前向后寻找，第一次找到要寻找的字符串之后提取之前的部分
		/// </summary>
		public static string 提取最之前(string 文本, string 寻找)
		{
			if (类型.为空(文本) || 类型.为空(寻找))
			{
				return "";
			}
			int num = Strings.InStr(文本, 寻找);
			if (num < 1)
			{
				return "";
			}
			return Strings.Left(文本, checked(num - 1));
		}

		/// <summary>
		/// 从后向前寻找，第一次找到要寻找的字符串之后提取之后的部分
		/// </summary>
		public static string 提取最之后(string 文本, string 寻找)
		{
			if (类型.为空(文本) || 类型.为空(寻找))
			{
				return "";
			}
			int num = Strings.InStrRev(文本, 寻找);
			if (num < 1)
			{
				return "";
			}
			return 文本.Substring(checked(num + 寻找.Length - 1));
		}

		/// <summary>
		/// 提取文本当中指定开头和结尾之间的文字，开头是第一次出现的那个开头，结尾是开头之后出现的第一次结尾
		/// </summary>
		public static string 提取之间(string 文本, string 开头, string 结尾)
		{
			if (类型.为空(文本) || 类型.为空(开头) || 类型.为空(结尾))
			{
				return "";
			}
			return 提取最之前(提取之后(文本, 开头), 结尾);
		}

		/// <summary>
		/// 从文本中去除对应的内容
		/// </summary>
		public static string 去除(string 文本, params string[] 内容)
		{
			foreach (string text in 内容)
			{
				if (包含(文本, text))
				{
					文本 = 文本.Replace(text, "");
				}
			}
			return 文本;
		}

		/// <summary>
		/// 去掉文本中的 CR LF 或者替换成指定内容
		/// </summary>
		public static string 去回车(string 文本, string 替换掉 = "")
		{
			return 替换(文本, "\r\n", 替换掉, "\r", 替换掉, "\n", 替换掉);
		}

		/// <summary>
		/// 去掉文本中，重复存在的字符
		/// </summary>
		public static string 去重(string 文本)
		{
			if (类型.为空(文本))
			{
				return "";
			}
			string text = "";
			foreach (char value in 文本)
			{
				if (!text.Contains(Conversions.ToString(value)))
				{
					text += Conversions.ToString(value);
				}
			}
			return text;
		}

		/// <summary>
		/// 没有 BOM 标识的 UTF-8 编码
		/// </summary>
		/// <returns></returns>
		public static Encoding 无BOM的UTF8编码()
		{
			if (_0024STATIC_0024无BOM的UTF8编码_002400128181_0024m_0024Init == null)
			{
				Interlocked.CompareExchange(ref _0024STATIC_0024无BOM的UTF8编码_002400128181_0024m_0024Init, new StaticLocalInitFlag(), null);
			}
			bool lockTaken = false;
			try
			{
				Monitor.Enter(_0024STATIC_0024无BOM的UTF8编码_002400128181_0024m_0024Init, ref lockTaken);
				if (_0024STATIC_0024无BOM的UTF8编码_002400128181_0024m_0024Init.State == 0)
				{
					_0024STATIC_0024无BOM的UTF8编码_002400128181_0024m_0024Init.State = 2;
					_0024STATIC_0024无BOM的UTF8编码_002400128181_0024m = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
				}
				else if (_0024STATIC_0024无BOM的UTF8编码_002400128181_0024m_0024Init.State == 2)
				{
					throw new IncompleteInitialization();
				}
			}
			finally
			{
				_0024STATIC_0024无BOM的UTF8编码_002400128181_0024m_0024Init.State = 1;
				if (lockTaken)
				{
					Monitor.Exit(_0024STATIC_0024无BOM的UTF8编码_002400128181_0024m_0024Init);
				}
			}
			return _0024STATIC_0024无BOM的UTF8编码_002400128181_0024m;
		}

		/// <summary>
		/// 把文本转为字节数组
		/// </summary>
		public static byte[] 文本转字节数组(string 文本, Encoding 编码 = null)
		{
			if (类型.为空(文本))
			{
				return null;
			}
			if (类型.为空(编码))
			{
				编码 = 无BOM的UTF8编码();
			}
			return 编码.GetBytes(文本);
		}

		/// <summary>
		/// 把字节数组转为文本，并且会进行标准化处理
		/// </summary>
		public static string 字节数组转文本(byte[] 字节数组, Encoding 编码 = null)
		{
			if (类型.为空(字节数组))
			{
				return "";
			}
			if (类型.为空(编码))
			{
				编码 = 无BOM的UTF8编码();
			}
			checked
			{
				int num = 字节数组.Length - 1;
				for (int i = 0; i <= num; i++)
				{
					int num2 = 字节数组[i];
					if (num2 < 31 && num2 != 9 && num2 != 10 && num2 != 13)
					{
						num2 = 32;
					}
				}
				string 文本 = 编码.GetString(字节数组);
				return 文本标准化(ref 文本);
			}
		}

		/// <summary>
		/// 如果文本里存在连续2个及以上个数的内容，就替换成只有一个
		/// </summary>
		public static string 去连续重复(string 文本, params string[] 内容)
		{
			foreach (string text in 内容)
			{
				string text2 = text + text;
				while (包含(文本, text2))
				{
					文本 = 替换(文本, text2, text);
				}
			}
			return 文本;
		}

		/// <summary>
		/// 把文本进行分割并生成列表，如果不包含间隔字符串，就返回本体，不保留中间的零长度内容，且默认自动trim每一项内容
		/// </summary>
		public static List<string> 分割(string 文本, string 间隔, bool trim内容 = true)
		{
			List<string> list = new List<string>();
			checked
			{
				if (!包含(文本, 间隔))
				{
					list.Add(文本);
				}
				else
				{
					文本 = 去连续重复(文本);
					int length = 间隔.Length;
					if (头(文本, 间隔))
					{
						文本 = 去左(文本, (uint)length);
					}
					if (!尾(文本, 间隔))
					{
						文本 += 间隔;
					}
					while (文本.Length > 0)
					{
						string text = 提取最之前(文本, 间隔);
						文本 = 去左(文本, (uint)(text.Length + length));
						if (trim内容)
						{
							text = Strings.Trim(text);
						}
						if (text.Length > 0)
						{
							list.Add(text);
						}
					}
				}
				return list;
			}
		}

		/// <summary>
		/// 把文本内的每一行都提取出来生成一个列表，默认把空的行也算在内
		/// </summary>
		public static List<string> 分行(string 文本, bool 去除空行 = false)
		{
			List<string> list = new List<string>();
			if (包含(文本标准化(ref 文本), "\r\n"))
			{
				if (!文本.EndsWith("\r\n"))
				{
					文本 += "\r\n";
				}
				while (包含(文本, "\r\n"))
				{
					string text = 提取最之前(文本, "\r\n");
					int length = text.Length;
					if (length > 0 || !去除空行)
					{
						list.Add(text);
					}
					文本 = 去左(文本, checked((uint)(length + 2)));
				}
			}
			else
			{
				list.Add(文本);
			}
			return list;
		}

		/// <summary>
		/// 筛选文本中的字符，如果不是规定的字符，那就去除
		/// </summary>
		public static string 筛选字符(string 文本, string 字符)
		{
			if (类型.为空(文本, 字符))
			{
				return "";
			}
			string text = "";
			IEnumerator enumerator = default(IEnumerator);
			try
			{
				enumerator = Regex.Matches(文本, "[" + Regex.Escape(字符) + "]*").GetEnumerator();
				while (enumerator.MoveNext())
				{
					Match match = (Match)enumerator.Current;
					text += match.ToString();
				}
			}
			finally
			{
				if (enumerator is IDisposable)
				{
					(enumerator as IDisposable).Dispose();
				}
			}
			return text;
		}

		/// <summary>
		/// 提取文字中的数字
		/// </summary>
		public static string 仅数字(string 文本)
		{
			return 筛选字符(文本, "0123456789");
		}

		/// <summary>
		/// 提取文字中的字母，大小写不限
		/// </summary>
		public static string 仅字母(string 文本)
		{
			return 筛选字符(文本, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
		}

		/// <summary>
		/// 提取文字中的大写字母
		/// </summary>
		public static string 仅大写字母(string 文本)
		{
			return 筛选字符(文本, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
		}

		/// <summary>
		/// 提取文字中的小写字母
		/// </summary>
		public static string 仅小写字母(string 文本)
		{
			return 筛选字符(文本, "abcdefghijklmnopqrstuvwxyz");
		}

		/// <summary>
		/// 把文本变成 "文本"
		/// </summary>
		public static string 引(string 文本)
		{
			return "\"" + 文本 + "\"";
		}

		/// <summary>
		/// 把字典的每一项的名字和值tostring都转成一行，连接符默认是:，然后拼在一起
		/// </summary>
		public static string 字典转文本(IDictionary 字典, string 连接符 = ":")
		{
			string text = "";
			if (字典.Count > 0)
			{
				IEnumerator enumerator = default(IEnumerator);
				try
				{
					enumerator = 字典.Keys.GetEnumerator();
					while (enumerator.MoveNext())
					{
						object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
						if (!类型.非空(RuntimeHelpers.GetObjectValue(objectValue)) && 类型.非空(RuntimeHelpers.GetObjectValue(字典[RuntimeHelpers.GetObjectValue(objectValue)])))
						{
							text = text + objectValue.ToString() + 连接符 + 字典[RuntimeHelpers.GetObjectValue(objectValue)].ToString() + "\r\n";
						}
					}
				}
				finally
				{
					if (enumerator is IDisposable)
					{
						(enumerator as IDisposable).Dispose();
					}
				}
			}
			return 去右(text, 2u);
		}

		/// <summary>
		/// 把列表的每一项的tostring都转成一行，然后拼在一起
		/// </summary>
		public static string 列表转文本(object 列表)
		{
			string text = "";
			if (Operators.ConditionalCompareObjectGreater(NewLateBinding.LateGet(列表, null, "Count", new object[0], null, null, null), 0, TextCompare: false))
			{
				IEnumerator enumerator = default(IEnumerator);
				try
				{
					enumerator = ((IEnumerable)列表).GetEnumerator();
					while (enumerator.MoveNext())
					{
						object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
						if (类型.非空(RuntimeHelpers.GetObjectValue(objectValue)))
						{
							text = text + objectValue.ToString() + "\r\n";
						}
					}
				}
				finally
				{
					if (enumerator is IDisposable)
					{
						(enumerator as IDisposable).Dispose();
					}
				}
			}
			return 去右(text, 2u);
		}

		/// <summary>
		/// 对字符串进行URLencode
		/// </summary>
		public static string URL编码(string 文本)
		{
			if (类型.为空(文本))
			{
				return "";
			}
			return HttpUtility.UrlEncode(文本);
		}

		/// <summary>
		/// 对字符串进行URLdncode
		/// </summary>
		public static string URL解码(string URL文本)
		{
			if (类型.为空(URL文本))
			{
				return "";
			}
			return HttpUtility.UrlDecode(URL文本);
		}

		/// <summary>
		/// 对转义后的文本进行反转义
		/// </summary>
		public static string 反转义(string 文本)
		{
			if (文本.Length < 3)
			{
				return 文本;
			}
			string 文本2 = 文本;
			文本2 = 替换(文本2, "\\r\\n", "\r\n", "\\r", "\r", "\\n", "\n", "\\/", "/", "\\t", "    ");
			IEnumerator enumerator = default(IEnumerator);
			try
			{
				enumerator = Regex.Matches(文本2, "\\\\u([0-9]|[a-f]){4}").GetEnumerator();
				while (enumerator.MoveNext())
				{
					Match match = (Match)enumerator.Current;
					string text = match.ToString();
					文本2 = 替换(文本2, text, Conversions.ToString(Strings.ChrW(Convert.ToInt32(右(text, 4u), 16))));
				}
			}
			finally
			{
				if (enumerator is IDisposable)
				{
					(enumerator as IDisposable).Dispose();
				}
			}
			return 文本标准化(ref 文本2);
		}

		/// <summary>
		/// 把数组里的每一项.ToString拼接起来，其中中间是指定的连接符
		/// </summary>
		public static string 数组拼接文本(string 连接符, params object[] 数组)
		{
			string text = "";
			checked
			{
				for (int i = 0; i < 数组.Length; i++)
				{
					object objectValue = RuntimeHelpers.GetObjectValue(数组[i]);
					text = text + objectValue.ToString() + 连接符;
				}
				return 去右(text, (uint)连接符.Length);
			}
		}

		/// <summary>
		/// 把数组里的每一项.ToString拼接起来
		/// </summary>
		public static string 数组拼接文本(string[] 数组)
		{
			return 数组拼接文本("", 数组);
		}

		/// <summary>
		/// 把文本重复几次后返回
		/// </summary>
		public static string 重复(string 文本, uint 次数)
		{
			if ((long)次数 < 1L || 类型.为空(文本))
			{
				return "";
			}
			string text = "";
			checked
			{
				int num = (int)次数;
				for (int i = 1; i <= num; i++)
				{
					text += 文本;
				}
				return text;
			}
		}

		/// <summary>
		/// 如果长度不足，就用指定的字符串补充在左边直到长度达标
		/// </summary>
		/// <returns></returns>
		public static string 补左(string 文本, uint 长度, string 补充 = " ")
		{
			if (类型.为空(补充) || (long)长度 < 1L)
			{
				return 文本;
			}
			while (文本.Length < 长度)
			{
				文本 = 补充 + 文本;
			}
			return 文本;
		}

		/// <summary>
		/// 如果长度不足，就用指定的字符串补充在右边直到长度达标
		/// </summary>
		/// <returns></returns>
		public static string 补右(string 文本, uint 长度, string 补充 = " ")
		{
			if (类型.为空(补充) || (long)长度 < 1L)
			{
				return 文本;
			}
			while (文本.Length < 长度)
			{
				文本 += 补充;
			}
			return 文本;
		}

		private static string MD粗体斜体(string m)
		{
			if (m.Length > 2)
			{
				m = 去连续重复(m, " ");
				m = 正则.替换(m, "\\*\\*(.+?)\\*\\*", "<b>$1</b>", "\\*(.+?)\\*", "<i>$1</i>", "!\\[(.*?)\\]\\((.+?)\\)", "<img alt='$1' src='$2'>");
				m = 正则.替换(m, "\\[(.*?)\\]\\((.+?)\\)", "<a href='$2'>$1</a>", "`+(.+?)`+", "<code>$1</code>");
			}
			return m;
		}

		/// <summary>
		/// 把Markdown文本转为HTML，不支持同一语法的多层嵌套（比如列表里面嵌套一层列表）
		/// </summary>
		public static string Markdown转HTML(string md)
		{
			if (md.Length > 2)
			{
				md = 正则.替换(md, "^ *$", "");
				md = 正则.高级分块(md, "(<(.+?) *[^>]*?>.*?</\\2>)", (_Closure_0024__._0024I43_002D0 != null) ? _Closure_0024__._0024I43_002D0 : (_Closure_0024__._0024I43_002D0 = ((string m) => 压缩HTML(m))), (_Closure_0024__._0024I43_002D1 != null) ? _Closure_0024__._0024I43_002D1 : (_Closure_0024__._0024I43_002D1 = delegate(string m)
				{
					string text3 = "";
					if (!m.EndsWith("\r\n"))
					{
						m += "\r\n";
					}
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					bool flag4 = false;
					bool flag5 = false;
					foreach (string item in 分行(m + "\r\n"))
					{
						string text4 = item;
						if (类型.为空(text4.Trim()))
						{
							if (flag5)
							{
								flag5 = false;
								text3 += "</p>";
							}
							else
							{
								text3 += "<br>";
							}
							if (flag2)
							{
								flag2 = false;
								text3 += "</ol>";
							}
							if (flag3)
							{
								flag3 = false;
								text3 += "</ul>";
							}
							if (flag4)
							{
								flag4 = false;
								text3 += "</blockquote>";
							}
						}
						else
						{
							if (text4.StartsWith(">") && !flag4)
							{
								flag4 = true;
								text3 += "<blockquote>";
							}
							if (flag4)
							{
								if (text4.StartsWith(">"))
								{
									text4 = 正则.去除(text4, "^>+");
								}
								else
								{
									flag4 = false;
									text3 += "</blockquote>";
								}
							}
							if (flag2 && !正则.包含(text4.Trim(), "^[0-9]+\\. "))
							{
								flag2 = false;
								text3 += "</ol>";
							}
							if (flag3 && !正则.包含(text4.Trim(), "^- "))
							{
								flag3 = false;
								text3 += "</ul>";
							}
							if (正则.包含(text4, "^\\#+ "))
							{
								if (flag5)
								{
									flag5 = false;
									text3 += "</p>";
								}
								int length = 提取最之前(text4, " ").Length;
								if (length <= 6)
								{
									text4 = 正则.去除(text4, "^\\#+ ", " \\#+$").Trim();
									string text5 = "h" + length.ToString() + ">";
									text4 = "<" + text5 + MD粗体斜体(text4) + "</" + text5;
								}
							}
							else if (正则.包含(text4.Trim(), "^-{3,}$"))
							{
								if (flag5)
								{
									flag5 = false;
									text3 += "</p>";
								}
								text4 = "<hr>";
							}
							else if (正则.包含(text4.Trim(), "```.*") && !flag)
							{
								if (flag5)
								{
									flag5 = false;
									text3 += "</p>";
								}
								string text5 = 去左(text4.Trim(), 3u);
								if (text5.Length > 0)
								{
									text5 = " class='" + text5 + "'";
								}
								text4 = "<pre><code" + text5 + ">";
								flag = true;
							}
							else if (Operators.CompareString(text4.Trim(), "```", TextCompare: false) != 0 && flag)
							{
								text4 = 替换(text4, " ", "&nbsp;") + "<br>";
							}
							else if (Operators.CompareString(text4.Trim(), "```", TextCompare: false) == 0 && flag)
							{
								flag = false;
								text4 = "</code></pre>";
							}
							else if (正则.包含(text4.Trim(), "^[0-9]+\\. "))
							{
								if (flag5)
								{
									flag5 = false;
									text3 += "</p>";
								}
								if (!flag2)
								{
									flag2 = true;
									text3 += "<ol>";
								}
								text4 = "<li>" + MD粗体斜体(提取之后(text4.Trim(), " ")) + "</li>";
							}
							else if (正则.包含(text4.Trim(), "^- "))
							{
								if (flag5)
								{
									flag5 = false;
									text3 += "</p>";
								}
								if (!flag3)
								{
									flag3 = true;
									text3 += "<ul>";
								}
								text4 = "<li>" + MD粗体斜体(提取之后(text4.Trim(), " ")) + "</li>";
							}
							else
							{
								if (!flag5)
								{
									text3 += "<p>";
									flag5 = true;
								}
								if (text4.EndsWith("  "))
								{
									text4 = text4.Trim() + "<br>";
								}
								text4 = MD粗体斜体(text4);
							}
							text3 += text4;
						}
					}
					text3 = 正则.替换(text3, "(<br>)*<hr>(<br>)*", "<hr>", "<br></code></pre>", "</code></pre>", "><br></", "></", "<br></", "</");
					if (!提取最之后(text3, "<p>").Contains("</p>"))
					{
						text3 += "</p>";
					}
					return text3;
				}));
				md = 正则.高级替换(md, "<code.*?>(.*?)</code>", (_Closure_0024__._0024I43_002D2 != null) ? _Closure_0024__._0024I43_002D2 : (_Closure_0024__._0024I43_002D2 = delegate(string m)
				{
					string text = 提取最之前(m, ">") + ">";
					string text2 = "</code>";
					string 文本 = checked(去右(去左(m, (uint)text.Length), (uint)text2.Length));
					文本 = 替换(文本, "<br>", "\r\n", "<", "&lt;", ">", "&gt;", "\r\n", "<br>");
					return text + 文本 + text2;
				}));
			}
			return md;
		}

		private static string 去除依附空格(string 文本, string 寻找)
		{
			foreach (char value in 寻找)
			{
				while (包含(文本, " " + Conversions.ToString(value), Conversions.ToString(value) + " "))
				{
					文本 = 替换(文本, " " + Conversions.ToString(value), Conversions.ToString(value), Conversions.ToString(value) + " ", Conversions.ToString(value));
				}
			}
			return 文本;
		}

		/// <summary>
		/// 对HTML进行压缩，请尽量保证提供的内容是正确的
		/// </summary>
		public static string 压缩HTML(string html)
		{
			if (html.Length < 4)
			{
				return html;
			}
			html = 正则.高级替换(文本标准化(ref html), ">[^<]*?</.+?>", (_Closure_0024__._0024I45_002D0 != null) ? _Closure_0024__._0024I45_002D0 : (_Closure_0024__._0024I45_002D0 = delegate(string m)
			{
				string text = 去右(提取最之后(m, "/").ToLower(), 1u);
				string text2 = 提取最之前(去左(m, 1u), "<");
				text2 = ((Operators.CompareString(text, "script", TextCompare: false) == 0) ? 压缩JS(text2) : ((Operators.CompareString(text, "style", TextCompare: false) != 0) ? 去连续重复(text2, " ") : 压缩CSS(text2)));
				return ">" + text2 + "</" + text + ">";
			}));
			html = 正则.高级替换(html, "('|\")[^<|^>]+?\\1", (_Closure_0024__._0024I45_002D1 != null) ? _Closure_0024__._0024I45_002D1 : (_Closure_0024__._0024I45_002D1 = ((string m) => 左(m, 1u) + 压缩JS(去右(去左(m, 1u), 1u)) + 右(m, 1u))));
			html = 正则.去除(html, "<!--([\\S|\\s]*?)-->", "\r\n");
			html = 去除依附空格(html, "<>");
			html = 正则.高级分块(html, "([\"|']).*?\\1", null, (_Closure_0024__._0024I45_002D2 != null) ? _Closure_0024__._0024I45_002D2 : (_Closure_0024__._0024I45_002D2 = ((string m) => 去除依附空格(正则.替换(m, " +", " "), "<>"))));
			return html;
		}

		/// <summary>
		/// 对CSS进行压缩，请尽量保证提供的内容是正确的
		/// </summary>
		public static string 压缩CSS(string css)
		{
			css = 文本标准化(ref css);
			return 去除依附空格(正则.去除(css, "\r\n", "/\\*.*?\\*/", " {2,}"), ",>:;{}()");
		}

		/// <summary>
		/// 对JavaScript进行压缩，请尽量保证提供的内容是正确的
		/// </summary>
		public static string 压缩JS(string js)
		{
			js = 正则.去除(文本标准化(ref js), "/\\*([\\S|\\s]*?)\\*/", "//.*[\\n|$]", "\r\n");
			js = 正则.高级分块(js, "([\"']).*?\\1", null, (_Closure_0024__._0024I47_002D0 != null) ? _Closure_0024__._0024I47_002D0 : (_Closure_0024__._0024I47_002D0 = ((string m) => 去除依附空格(正则.替换(m, " +", " "), "=(){},;\""))));
			return js;
		}

		/// <summary>
		/// 比较两个文本，一个字一个字的比，如果A比B小，就返回-1，等于返回0，大于返回1
		/// </summary>
		public static int 比较文本(string A, string B)
		{
			if (类型.为空(A, B))
			{
				if (类型.为空全部(A, B))
				{
					return 0;
				}
				if (类型.为空(A))
				{
					return -1;
				}
				if (类型.为空(B))
				{
					return 1;
				}
			}
			checked
			{
				int num = Math.Min(A.Length, B.Length) - 1;
				if (A.Length == B.Length)
				{
					if (Operators.CompareString(A, B, TextCompare: false) == 0)
					{
						return 0;
					}
				}
				else
				{
					if (A.Length < B.Length && Operators.CompareString(左(A, (uint)B.Length), B, TextCompare: false) == 0)
					{
						return -1;
					}
					if (A.Length > B.Length && Operators.CompareString(左(B, (uint)A.Length), A, TextCompare: false) == 0)
					{
						return 1;
					}
				}
				int num2 = num;
				for (int i = 0; i <= num2; i++)
				{
					int num3 = A[i];
					int num4 = B[i];
					if (num3 > num4)
					{
						return 1;
					}
					if (num3 < num4)
					{
						return -1;
					}
				}
				return 0;
			}
		}
	}
}
