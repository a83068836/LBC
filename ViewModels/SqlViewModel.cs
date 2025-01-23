using LBC.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebFirst.Entities;
using SqlSugar;
using System.Diagnostics;
//using static System.Runtime.InteropServices.Marshalling.IIUnknownCacheStrategy;
using System.ComponentModel;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using FilterTreeViewLib.ViewModels;
using UnitComboLib.Command;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using Win32;
using System.Data.OleDb;
using System.Data;
using System.Dynamic;
using TextEditLib.Class;
using LBC.Views;
using LBC.Behaviors;
using LBC.Converters;
using System.Xml.Linq;
using AngleSharp.Dom;
using Visibility = System.Windows.Visibility;
using Microsoft.Xaml.Behaviors;
using System.Windows.Forms;
using Masuit.Tools;
using GongSolutions.Wpf.DragDrop.Utilities;
using System.Windows.Diagnostics;

namespace LBC.ViewModels
{

    public class SqlViewModel : ViewModelBases
    {
        private Dictionary<int[], Dictionary<string, string>> _columnHeaders = new Dictionary<int[], Dictionary<string, string>>
{
    {
        new int[]{ 5,6}, new Dictionary<string, string>
        {
            { "idx", "序号" },
    { "name", "名称" },
    { "StdMode", "分类" },
    { "Shape", "装备外观" },
    { "Weight", "重量" },
    { "Anicount", "隐藏属性" },
    { "Source", "神圣" },
    { "Reserved", "特殊属性" },
    { "Looks", "内观" },
    { "DuraMax", "持久" },
    { "Ac", "幸运" },
    { "AC2", "准确" },
    { "Mac", "诅咒" },
    { "Mac2", "攻速" },
    { "Dc", "攻击下限" },
    { "Dc2", "攻击上限" },
    { "Mc", "魔法下限" },
    { "Mc2", "魔法上限" },
    { "Sc", "道术下限" },
    { "Sc2", "道术上限" },
    { "Need", "特殊条件" },
    { "NeedLevel", "需要等级" },
    { "Price", "出售价格" },
    { "Stock", "库存量" },
    { "Color", "颜色" },
    { "OverLap", "无" },
    { "HP", "血量提升" },
    { "MP", "蓝量提升" },
    { "Job", "职业" },
    { "Value1", "暴击几率增加" },
    { "Value2", "增加攻击伤害" },
    { "Value3", "物理伤害减少" },
    { "Value4", "魔法伤害减少" },
    { "Value5", "忽视目标防御" },
    { "Value6", "所有伤害反弹" },
    { "Value7", "增加目标暴率" },
    { "Value8", "人物体力增加" },
    { "Value9", "人物魔力增加" },
    { "Value10", "怒气恢复增加" },
    { "Value11", "合击攻击增加" },
    { "Value12", "Value12" },
    { "Value13", "Value13" },
    { "Value14", "Value14" },
    { "Value15", "Value15" },
    { "Value16", "Value16" },
    { "Value17", "Value17" },
    { "Value18", "Value18" },
    { "Value19", "Value19" },
    { "Value20", "Value20" },
    { "Value21", "Value21" },
    { "Value22", "Value22" },
    { "Value23", "Value23" },
    { "Value24", "Value24" },
    { "Value25", "Value25" },
    { "Value26", "Value26" },
    { "Value27", "Value27" },
    { "Value28", "Value28" },
    { "Value29", "Value29" },
    { "Value30", "Value30" },
    { "InsuranceGold", "投保金额" },
    { "InsuranceCurrency", "保金类型" },
    { "element", "暴击几率增加" },
    { "element1", "增加攻击伤害" },
    { "element2", "物理伤害减少" },
    { "element3", "魔法伤害减少" },
    { "element4", "忽视目标防御" },
    { "element5", "所有伤害反弹" },
    { "element6", "增加杀人暴率" },
    { "element7", "人物体力增加" },
    { "element8", "人物魔力增加" },
    { "element9", "怒气恢复增加" },
    { "element10", "合击攻击增加" },
    { "element11", "增加杀怪暴率" },
    { "element12", "增加防爆几率" },
    { "element13", "增加防止麻痹" },
    { "element14", "增加防止护身" },
    { "element15", "增加防止复活" },
    { "element16", "增加防止全毒" },
    { "element17", "增加防止诱惑" },
    { "element18", "增加防止火墙" },
    { "element19", "增加防止冰冻" },
    { "element20", "增加防止蛛网" },
    { "element21", "致命一击几率" },
    { "element22", "致命一击伤害增加" },
    { "element23", "致命一击防御" },
    { "element24", "暴击抗性" },
    { "element25", "攻击伤害抗性" },
    { "element26", "杀怪经验倍率" }
        }
    },
    {
        new int[]{ 15}, new Dictionary<string, string>
        {

            { "idx", "序号" },
    { "name", "名称" },
    { "StdMode", "分类" },
    { "Shape", "隐藏属性" },
    { "Weight", "重量" },
    { "Anicount", "附加属性" },
    { "Source", "无" },
    { "Reserved", "特殊属性" },
    { "Looks", "内观" },
    { "DuraMax", "持久" },
    { "Ac", "防御下限" },
    { "AC2", "防御上限" },
    { "Mac", "魔御下限" },
    { "Mac2", "魔御上限" },
    { "Dc", "攻击下限" },
    { "Dc2", "攻击上限" },
    { "Mc", "魔法下限" },
    { "Mc2", "魔法上限" },
    { "Sc", "道术下限" },
    { "Sc2", "道术上限" },
    { "Need", "特殊条件" },
    { "NeedLevel", "需要等级" },
    { "Price", "出售价格" },
    { "Stock", "库存量" },
    { "Color", "颜色" },
    { "OverLap", "无" },
    { "HP", "血量提升" },
    { "MP", "蓝量提升" },
    { "Job", "职业" },
    { "Value1", "暴击几率增加" },
    { "Value2", "增加攻击伤害" },
    { "Value3", "物理伤害减少" },
    { "Value4", "魔法伤害减少" },
    { "Value5", "忽视目标防御" },
    { "Value6", "所有伤害反弹" },
    { "Value7", "增加目标暴率" },
    { "Value8", "人物体力增加" },
    { "Value9", "人物魔力增加" },
    { "Value10", "怒气恢复增加" },
    { "Value11", "合击攻击增加" },
    { "Value12", "Value12" },
    { "Value13", "Value13" },
    { "Value14", "Value14" },
    { "Value15", "Value15" },
    { "Value16", "Value16" },
    { "Value17", "Value17" },
    { "Value18", "Value18" },
    { "Value19", "Value19" },
    { "Value20", "Value20" },
    { "Value21", "Value21" },
    { "Value22", "Value22" },
    { "Value23", "Value23" },
    { "Value24", "Value24" },
    { "Value25", "Value25" },
    { "Value26", "Value26" },
    { "Value27", "Value27" },
    { "Value28", "Value28" },
    { "Value29", "Value29" },
    { "Value30", "Value30" },
    { "InsuranceGold", "投保金额" },
    { "InsuranceCurrency", "保金类型" },
    { "element", "暴击几率增加" },
    { "element1", "增加攻击伤害" },
    { "element2", "物理伤害减少" },
    { "element3", "魔法伤害减少" },
    { "element4", "忽视目标防御" },
    { "element5", "所有伤害反弹" },
    { "element6", "增加杀人暴率" },
    { "element7", "人物体力增加" },
    { "element8", "人物魔力增加" },
    { "element9", "怒气恢复增加" },
    { "element10", "合击攻击增加" },
    { "element11", "增加杀怪暴率" },
    { "element12", "增加防爆几率" },
    { "element13", "增加防止麻痹" },
    { "element14", "增加防止护身" },
    { "element15", "增加防止复活" },
    { "element16", "增加防止全毒" },
    { "element17", "增加防止诱惑" },
    { "element18", "增加防止火墙" },
    { "element19", "增加防止冰冻" },
    { "element20", "增加防止蛛网" },
    { "element21", "致命一击几率" },
    { "element22", "致命一击伤害增加" },
    { "element23", "致命一击防御" },
    { "element24", "暴击抗性" },
    { "element25", "攻击伤害抗性" },
    { "element26", "杀怪经验倍率" }
        }
    },
    {
        new int[]{ 10,11}, new Dictionary<string, string>
        {

            { "idx", "序号" },
    { "name", "名称" },
    { "StdMode", "分类" },
    { "Shape", "衣服外观" },
    { "Weight", "重量" },
    { "Anicount", "翅膀" },
    { "Source", "无" },
    { "Reserved", "特殊属性" },
    { "Looks", "内观" },
    { "DuraMax", "持久" },
    { "Ac", "防御下限" },
    { "AC2", "防御上限" },
    { "Mac", "魔御下限" },
    { "Mac2", "魔御上限" },
    { "Dc", "攻击下限" },
    { "Dc2", "攻击上限" },
    { "Mc", "魔法下限" },
    { "Mc2", "魔法上限" },
    { "Sc", "道术下限" },
    { "Sc2", "道术上限" },
    { "Need", "特殊条件" },
    { "NeedLevel", "需要等级" },
    { "Price", "出售价格" },
    { "Stock", "库存量" },
    { "Color", "颜色" },
    { "OverLap", "无" },
    { "HP", "血量提升" },
    { "MP", "蓝量提升" },
    { "Job", "职业" },
    { "Value1", "暴击几率增加" },
    { "Value2", "增加攻击伤害" },
    { "Value3", "物理伤害减少" },
    { "Value4", "魔法伤害减少" },
    { "Value5", "忽视目标防御" },
    { "Value6", "所有伤害反弹" },
    { "Value7", "增加目标暴率" },
    { "Value8", "人物体力增加" },
    { "Value9", "人物魔力增加" },
    { "Value10", "怒气恢复增加" },
    { "Value11", "合击攻击增加" },
    { "Value12", "Value12" },
    { "Value13", "Value13" },
    { "Value14", "Value14" },
    { "Value15", "Value15" },
    { "Value16", "Value16" },
    { "Value17", "Value17" },
    { "Value18", "Value18" },
    { "Value19", "Value19" },
    { "Value20", "Value20" },
    { "Value21", "Value21" },
    { "Value22", "Value22" },
    { "Value23", "Value23" },
    { "Value24", "Value24" },
    { "Value25", "Value25" },
    { "Value26", "Value26" },
    { "Value27", "Value27" },
    { "Value28", "Value28" },
    { "Value29", "Value29" },
    { "Value30", "Value30" },
    { "InsuranceGold", "投保金额" },
    { "InsuranceCurrency", "保金类型" },
    { "element", "暴击几率增加" },
    { "element1", "增加攻击伤害" },
    { "element2", "物理伤害减少" },
    { "element3", "魔法伤害减少" },
    { "element4", "忽视目标防御" },
    { "element5", "所有伤害反弹" },
    { "element6", "增加杀人暴率" },
    { "element7", "人物体力增加" },
    { "element8", "人物魔力增加" },
    { "element9", "怒气恢复增加" },
    { "element10", "合击攻击增加" },
    { "element11", "增加杀怪暴率" },
    { "element12", "增加防爆几率" },
    { "element13", "增加防止麻痹" },
    { "element14", "增加防止护身" },
    { "element15", "增加防止复活" },
    { "element16", "增加防止全毒" },
    { "element17", "增加防止诱惑" },
    { "element18", "增加防止火墙" },
    { "element19", "增加防止冰冻" },
    { "element20", "增加防止蛛网" },
    { "element21", "致命一击几率" },
    { "element22", "致命一击伤害增加" },
    { "element23", "致命一击防御" },
    { "element24", "暴击抗性" },
    { "element25", "攻击伤害抗性" },
    { "element26", "杀怪经验倍率" }
        }
    },{
        new int[]{ 16,48}, new Dictionary<string, string>
        {

            { "idx", "序号" },
    { "name", "名称" },
    { "StdMode", "分类" },
    { "Shape", "无" },
    { "Weight", "重量" },
    { "Anicount", "触发编号" },
    { "Source", "无" },
    { "Reserved", "无" },
    { "Looks", "内观" },
    { "DuraMax", "持久" },
    { "Ac", "防御下限" },
    { "AC2", "防御上限" },
    { "Mac", "魔御下限" },
    { "Mac2", "魔御上限" },
    { "Dc", "攻击下限" },
    { "Dc2", "攻击上限" },
    { "Mc", "魔法下限" },
    { "Mc2", "魔法上限" },
    { "Sc", "道术下限" },
    { "Sc2", "道术上限" },
    { "Need", "特殊条件" },
    { "NeedLevel", "需要等级" },
    { "Price", "出售价格" },
    { "Stock", "库存量" },
    { "Color", "颜色" },
    { "OverLap", "无" },
    { "HP", "血量提升" },
    { "MP", "蓝量提升" },
    { "Job", "职业" },
    { "Value1", "暴击几率增加" },
    { "Value2", "增加攻击伤害" },
    { "Value3", "物理伤害减少" },
    { "Value4", "魔法伤害减少" },
    { "Value5", "忽视目标防御" },
    { "Value6", "所有伤害反弹" },
    { "Value7", "增加目标暴率" },
    { "Value8", "人物体力增加" },
    { "Value9", "人物魔力增加" },
    { "Value10", "怒气恢复增加" },
    { "Value11", "合击攻击增加" },
    { "Value12", "Value12" },
    { "Value13", "Value13" },
    { "Value14", "Value14" },
    { "Value15", "Value15" },
    { "Value16", "Value16" },
    { "Value17", "Value17" },
    { "Value18", "Value18" },
    { "Value19", "Value19" },
    { "Value20", "Value20" },
    { "Value21", "Value21" },
    { "Value22", "Value22" },
    { "Value23", "Value23" },
    { "Value24", "Value24" },
    { "Value25", "Value25" },
    { "Value26", "Value26" },
    { "Value27", "Value27" },
    { "Value28", "Value28" },
    { "Value29", "Value29" },
    { "Value30", "Value30" },
    { "InsuranceGold", "投保金额" },
    { "InsuranceCurrency", "保金类型" },
    { "element", "暴击几率增加" },
    { "element1", "增加攻击伤害" },
    { "element2", "物理伤害减少" },
    { "element3", "魔法伤害减少" },
    { "element4", "忽视目标防御" },
    { "element5", "所有伤害反弹" },
    { "element6", "增加杀人暴率" },
    { "element7", "人物体力增加" },
    { "element8", "人物魔力增加" },
    { "element9", "怒气恢复增加" },
    { "element10", "合击攻击增加" },
    { "element11", "增加杀怪暴率" },
    { "element12", "增加防爆几率" },
    { "element13", "增加防止麻痹" },
    { "element14", "增加防止护身" },
    { "element15", "增加防止复活" },
    { "element16", "增加防止全毒" },
    { "element17", "增加防止诱惑" },
    { "element18", "增加防止火墙" },
    { "element19", "增加防止冰冻" },
    { "element20", "增加防止蛛网" },
    { "element21", "致命一击几率" },
    { "element22", "致命一击伤害增加" },
    { "element23", "致命一击防御" },
    { "element24", "暴击抗性" },
    { "element25", "攻击伤害抗性" },
    { "element26", "杀怪经验倍率" }
        }
    },{
        new int[]{ 30}, new Dictionary<string, string>
        {

            { "idx", "序号" },
    { "name", "名称" },
    { "StdMode", "分类" },
    { "Shape", "勋章翅膀" },
    { "Weight", "重量" },
    { "Anicount", "隐藏属性" },
    { "Source", "是否掉持久" },
    { "Reserved", "特殊属性" },
    { "Looks", "内观" },
    { "DuraMax", "持久" },
    { "Ac", "防御下限" },
    { "AC2", "防御上限" },
    { "Mac", "魔御下限" },
    { "Mac2", "魔御上限" },
    { "Dc", "攻击下限" },
    { "Dc2", "攻击上限" },
    { "Mc", "魔法下限" },
    { "Mc2", "魔法上限" },
    { "Sc", "道术下限" },
    { "Sc2", "道术上限" },
    { "Need", "特殊条件" },
    { "NeedLevel", "需要等级" },
    { "Price", "出售价格" },
    { "Stock", "库存量" },
    { "Color", "颜色" },
    { "OverLap", "无" },
    { "HP", "血量提升" },
    { "MP", "蓝量提升" },
    { "Job", "职业" },
    { "Value1", "暴击几率增加" },
    { "Value2", "增加攻击伤害" },
    { "Value3", "物理伤害减少" },
    { "Value4", "魔法伤害减少" },
    { "Value5", "忽视目标防御" },
    { "Value6", "所有伤害反弹" },
    { "Value7", "增加目标暴率" },
    { "Value8", "人物体力增加" },
    { "Value9", "人物魔力增加" },
    { "Value10", "怒气恢复增加" },
    { "Value11", "合击攻击增加" },
    { "Value12", "Value12" },
    { "Value13", "Value13" },
    { "Value14", "Value14" },
    { "Value15", "Value15" },
    { "Value16", "Value16" },
    { "Value17", "Value17" },
    { "Value18", "Value18" },
    { "Value19", "Value19" },
    { "Value20", "Value20" },
    { "Value21", "Value21" },
    { "Value22", "Value22" },
    { "Value23", "Value23" },
    { "Value24", "Value24" },
    { "Value25", "Value25" },
    { "Value26", "Value26" },
    { "Value27", "Value27" },
    { "Value28", "Value28" },
    { "Value29", "Value29" },
    { "Value30", "Value30" },
    { "InsuranceGold", "投保金额" },
    { "InsuranceCurrency", "保金类型" },
    { "element", "暴击几率增加" },
    { "element1", "增加攻击伤害" },
    { "element2", "物理伤害减少" },
    { "element3", "魔法伤害减少" },
    { "element4", "忽视目标防御" },
    { "element5", "所有伤害反弹" },
    { "element6", "增加杀人暴率" },
    { "element7", "人物体力增加" },
    { "element8", "人物魔力增加" },
    { "element9", "怒气恢复增加" },
    { "element10", "合击攻击增加" },
    { "element11", "增加杀怪暴率" },
    { "element12", "增加防爆几率" },
    { "element13", "增加防止麻痹" },
    { "element14", "增加防止护身" },
    { "element15", "增加防止复活" },
    { "element16", "增加防止全毒" },
    { "element17", "增加防止诱惑" },
    { "element18", "增加防止火墙" },
    { "element19", "增加防止冰冻" },
    { "element20", "增加防止蛛网" },
    { "element21", "致命一击几率" },
    { "element22", "致命一击伤害增加" },
    { "element23", "致命一击防御" },
    { "element24", "暴击抗性" },
    { "element25", "攻击伤害抗性" },
    { "element26", "杀怪经验倍率" }
        }
    },{
        new int[]{ 19,22}, new Dictionary<string, string>
        {

            { "idx", "序号" },
    { "name", "名称" },
    { "StdMode", "分类" },
    { "Shape", "隐藏属性" },
    { "Weight", "重量" },
    { "Anicount", "附加属性" },
    { "Source", "无" },
    { "Reserved", "特殊属性" },
    { "Looks", "内观" },
    { "DuraMax", "持久" },
    { "Ac", "无" },
    { "AC2", "魔法躲避" },
    { "Mac", "诅咒" },
    { "Mac2", "幸运" },
    { "Dc", "攻击下限" },
    { "Dc2", "攻击上限" },
    { "Mc", "魔法下限" },
    { "Mc2", "魔法上限" },
    { "Sc", "道术下限" },
    { "Sc2", "道术上限" },
    { "Need", "特殊条件" },
    { "NeedLevel", "需要等级" },
    { "Price", "出售价格" },
    { "Stock", "库存量" },
    { "Color", "颜色" },
    { "OverLap", "无" },
    { "HP", "血量提升" },
    { "MP", "蓝量提升" },
    { "Job", "职业" },
    { "Value1", "暴击几率增加" },
    { "Value2", "增加攻击伤害" },
    { "Value3", "物理伤害减少" },
    { "Value4", "魔法伤害减少" },
    { "Value5", "忽视目标防御" },
    { "Value6", "所有伤害反弹" },
    { "Value7", "增加目标暴率" },
    { "Value8", "人物体力增加" },
    { "Value9", "人物魔力增加" },
    { "Value10", "怒气恢复增加" },
    { "Value11", "合击攻击增加" },
    { "Value12", "Value12" },
    { "Value13", "Value13" },
    { "Value14", "Value14" },
    { "Value15", "Value15" },
    { "Value16", "Value16" },
    { "Value17", "Value17" },
    { "Value18", "Value18" },
    { "Value19", "Value19" },
    { "Value20", "Value20" },
    { "Value21", "Value21" },
    { "Value22", "Value22" },
    { "Value23", "Value23" },
    { "Value24", "Value24" },
    { "Value25", "Value25" },
    { "Value26", "Value26" },
    { "Value27", "Value27" },
    { "Value28", "Value28" },
    { "Value29", "Value29" },
    { "Value30", "Value30" },
    { "InsuranceGold", "投保金额" },
    { "InsuranceCurrency", "保金类型" },
    { "element", "暴击几率增加" },
    { "element1", "增加攻击伤害" },
    { "element2", "物理伤害减少" },
    { "element3", "魔法伤害减少" },
    { "element4", "忽视目标防御" },
    { "element5", "所有伤害反弹" },
    { "element6", "增加杀人暴率" },
    { "element7", "人物体力增加" },
    { "element8", "人物魔力增加" },
    { "element9", "怒气恢复增加" },
    { "element10", "合击攻击增加" },
    { "element11", "增加杀怪暴率" },
    { "element12", "增加防爆几率" },
    { "element13", "增加防止麻痹" },
    { "element14", "增加防止护身" },
    { "element15", "增加防止复活" },
    { "element16", "增加防止全毒" },
    { "element17", "增加防止诱惑" },
    { "element18", "增加防止火墙" },
    { "element19", "增加防止冰冻" },
    { "element20", "增加防止蛛网" },
    { "element21", "致命一击几率" },
    { "element22", "致命一击伤害增加" },
    { "element23", "致命一击防御" },
    { "element24", "暴击抗性" },
    { "element25", "攻击伤害抗性" },
    { "element26", "杀怪经验倍率" }
        }
    },{
        new int[]{ 20,24}, new Dictionary<string, string>
        {

            { "idx", "序号" },
    { "name", "名称" },
    { "StdMode", "分类" },
    { "Shape", "隐藏属性" },
    { "Weight", "重量" },
    { "Anicount", "附加属性" },
    { "Source", "无" },
    { "Reserved", "特殊属性" },
    { "Looks", "内观" },
    { "DuraMax", "持久" },
    { "Ac", "无" },
    { "AC2", "准确" },
    { "Mac", "无" },
    { "Mac2", "敏捷" },
    { "Dc", "攻击下限" },
    { "Dc2", "攻击上限" },
    { "Mc", "魔法下限" },
    { "Mc2", "魔法上限" },
    { "Sc", "道术下限" },
    { "Sc2", "道术上限" },
    { "Need", "特殊条件" },
    { "NeedLevel", "需要等级" },
    { "Price", "出售价格" },
    { "Stock", "库存量" },
    { "Color", "颜色" },
    { "OverLap", "无" },
    { "HP", "血量提升" },
    { "MP", "蓝量提升" },
    { "Job", "职业" },
    { "Value1", "暴击几率增加" },
    { "Value2", "增加攻击伤害" },
    { "Value3", "物理伤害减少" },
    { "Value4", "魔法伤害减少" },
    { "Value5", "忽视目标防御" },
    { "Value6", "所有伤害反弹" },
    { "Value7", "增加目标暴率" },
    { "Value8", "人物体力增加" },
    { "Value9", "人物魔力增加" },
    { "Value10", "怒气恢复增加" },
    { "Value11", "合击攻击增加" },
    { "Value12", "Value12" },
    { "Value13", "Value13" },
    { "Value14", "Value14" },
    { "Value15", "Value15" },
    { "Value16", "Value16" },
    { "Value17", "Value17" },
    { "Value18", "Value18" },
    { "Value19", "Value19" },
    { "Value20", "Value20" },
    { "Value21", "Value21" },
    { "Value22", "Value22" },
    { "Value23", "Value23" },
    { "Value24", "Value24" },
    { "Value25", "Value25" },
    { "Value26", "Value26" },
    { "Value27", "Value27" },
    { "Value28", "Value28" },
    { "Value29", "Value29" },
    { "Value30", "Value30" },
    { "InsuranceGold", "投保金额" },
    { "InsuranceCurrency", "保金类型" },
    { "element", "暴击几率增加" },
    { "element1", "增加攻击伤害" },
    { "element2", "物理伤害减少" },
    { "element3", "魔法伤害减少" },
    { "element4", "忽视目标防御" },
    { "element5", "所有伤害反弹" },
    { "element6", "增加杀人暴率" },
    { "element7", "人物体力增加" },
    { "element8", "人物魔力增加" },
    { "element9", "怒气恢复增加" },
    { "element10", "合击攻击增加" },
    { "element11", "增加杀怪暴率" },
    { "element12", "增加防爆几率" },
    { "element13", "增加防止麻痹" },
    { "element14", "增加防止护身" },
    { "element15", "增加防止复活" },
    { "element16", "增加防止全毒" },
    { "element17", "增加防止诱惑" },
    { "element18", "增加防止火墙" },
    { "element19", "增加防止冰冻" },
    { "element20", "增加防止蛛网" },
    { "element21", "致命一击几率" },
    { "element22", "致命一击伤害增加" },
    { "element23", "致命一击防御" },
    { "element24", "暴击抗性" },
    { "element25", "攻击伤害抗性" },
    { "element26", "杀怪经验倍率" }
        }
    },{
        new int[]{ 21}, new Dictionary<string, string>
        {

            { "idx", "序号" },
    { "name", "名称" },
    { "StdMode", "分类" },
    { "Shape", "隐藏属性" },
    { "Weight", "重量" },
    { "Anicount", "附加属性" },
    { "Source", "无" },
    { "Reserved", "特殊属性" },
    { "Looks", "内观" },
    { "DuraMax", "持久" },
    { "Ac", "攻击速度+" },
    { "AC2", "体力恢复" },
    { "Mac", "攻击速度-" },
    { "Mac2", "魔法恢复" },
    { "Dc", "攻击下限" },
    { "Dc2", "攻击上限" },
    { "Mc", "魔法下限" },
    { "Mc2", "魔法上限" },
    { "Sc", "道术下限" },
    { "Sc2", "道术上限" },
    { "Need", "特殊条件" },
    { "NeedLevel", "需要等级" },
    { "Price", "出售价格" },
    { "Stock", "库存量" },
    { "Color", "颜色" },
    { "OverLap", "无" },
    { "HP", "血量提升" },
    { "MP", "蓝量提升" },
    { "Job", "职业" },
    { "Value1", "暴击几率增加" },
    { "Value2", "增加攻击伤害" },
    { "Value3", "物理伤害减少" },
    { "Value4", "魔法伤害减少" },
    { "Value5", "忽视目标防御" },
    { "Value6", "所有伤害反弹" },
    { "Value7", "增加目标暴率" },
    { "Value8", "人物体力增加" },
    { "Value9", "人物魔力增加" },
    { "Value10", "怒气恢复增加" },
    { "Value11", "合击攻击增加" },
    { "Value12", "Value12" },
    { "Value13", "Value13" },
    { "Value14", "Value14" },
    { "Value15", "Value15" },
    { "Value16", "Value16" },
    { "Value17", "Value17" },
    { "Value18", "Value18" },
    { "Value19", "Value19" },
    { "Value20", "Value20" },
    { "Value21", "Value21" },
    { "Value22", "Value22" },
    { "Value23", "Value23" },
    { "Value24", "Value24" },
    { "Value25", "Value25" },
    { "Value26", "Value26" },
    { "Value27", "Value27" },
    { "Value28", "Value28" },
    { "Value29", "Value29" },
    { "Value30", "Value30" },
    { "InsuranceGold", "投保金额" },
    { "InsuranceCurrency", "保金类型" },
    { "element", "暴击几率增加" },
    { "element1", "增加攻击伤害" },
    { "element2", "物理伤害减少" },
    { "element3", "魔法伤害减少" },
    { "element4", "忽视目标防御" },
    { "element5", "所有伤害反弹" },
    { "element6", "增加杀人暴率" },
    { "element7", "人物体力增加" },
    { "element8", "人物魔力增加" },
    { "element9", "怒气恢复增加" },
    { "element10", "合击攻击增加" },
    { "element11", "增加杀怪暴率" },
    { "element12", "增加防爆几率" },
    { "element13", "增加防止麻痹" },
    { "element14", "增加防止护身" },
    { "element15", "增加防止复活" },
    { "element16", "增加防止全毒" },
    { "element17", "增加防止诱惑" },
    { "element18", "增加防止火墙" },
    { "element19", "增加防止冰冻" },
    { "element20", "增加防止蛛网" },
    { "element21", "致命一击几率" },
    { "element22", "致命一击伤害增加" },
    { "element23", "致命一击防御" },
    { "element24", "暴击抗性" },
    { "element25", "攻击伤害抗性" },
    { "element26", "杀怪经验倍率" }
        }
    },{
        new int[]{ 23}, new Dictionary<string, string>
        {

            { "idx", "序号" },
    { "name", "名称" },
    { "StdMode", "分类" },
    { "Shape", "隐藏属性" },
    { "Weight", "重量" },
    { "Anicount", "附加属性" },
    { "Source", "无" },
    { "Reserved", "特殊属性" },
    { "Looks", "内观" },
    { "DuraMax", "持久" },
    { "Ac", "无" },
    { "AC2", "中毒躲避+" },
    { "Mac", "无" },
    { "Mac2", "中毒恢复+" },
    { "Dc", "攻击下限" },
    { "Dc2", "攻击上限" },
    { "Mc", "魔法下限" },
    { "Mc2", "魔法上限" },
    { "Sc", "道术下限" },
    { "Sc2", "道术上限" },
    { "Need", "特殊条件" },
    { "NeedLevel", "需要等级" },
    { "Price", "出售价格" },
    { "Stock", "库存量" },
    { "Color", "颜色" },
    { "OverLap", "无" },
    { "HP", "血量提升" },
    { "MP", "蓝量提升" },
    { "Job", "职业" },
    { "Value1", "暴击几率增加" },
    { "Value2", "增加攻击伤害" },
    { "Value3", "物理伤害减少" },
    { "Value4", "魔法伤害减少" },
    { "Value5", "忽视目标防御" },
    { "Value6", "所有伤害反弹" },
    { "Value7", "增加目标暴率" },
    { "Value8", "人物体力增加" },
    { "Value9", "人物魔力增加" },
    { "Value10", "怒气恢复增加" },
    { "Value11", "合击攻击增加" },
    { "Value12", "Value12" },
    { "Value13", "Value13" },
    { "Value14", "Value14" },
    { "Value15", "Value15" },
    { "Value16", "Value16" },
    { "Value17", "Value17" },
    { "Value18", "Value18" },
    { "Value19", "Value19" },
    { "Value20", "Value20" },
    { "Value21", "Value21" },
    { "Value22", "Value22" },
    { "Value23", "Value23" },
    { "Value24", "Value24" },
    { "Value25", "Value25" },
    { "Value26", "Value26" },
    { "Value27", "Value27" },
    { "Value28", "Value28" },
    { "Value29", "Value29" },
    { "Value30", "Value30" },
    { "InsuranceGold", "投保金额" },
    { "InsuranceCurrency", "保金类型" },
    { "element", "暴击几率增加" },
    { "element1", "增加攻击伤害" },
    { "element2", "物理伤害减少" },
    { "element3", "魔法伤害减少" },
    { "element4", "忽视目标防御" },
    { "element5", "所有伤害反弹" },
    { "element6", "增加杀人暴率" },
    { "element7", "人物体力增加" },
    { "element8", "人物魔力增加" },
    { "element9", "怒气恢复增加" },
    { "element10", "合击攻击增加" },
    { "element11", "增加杀怪暴率" },
    { "element12", "增加防爆几率" },
    { "element13", "增加防止麻痹" },
    { "element14", "增加防止护身" },
    { "element15", "增加防止复活" },
    { "element16", "增加防止全毒" },
    { "element17", "增加防止诱惑" },
    { "element18", "增加防止火墙" },
    { "element19", "增加防止冰冻" },
    { "element20", "增加防止蛛网" },
    { "element21", "致命一击几率" },
    { "element22", "致命一击伤害增加" },
    { "element23", "致命一击防御" },
    { "element24", "暴击抗性" },
    { "element25", "攻击伤害抗性" },
    { "element26", "杀怪经验倍率" }
        }
    },{
        new int[]{ 52,62,63,64,28,26,65,90}, new Dictionary<string, string>
        {

            { "idx", "序号" },
    { "name", "名称" },
    { "StdMode", "分类" },
    { "Shape", "隐藏属性" },
    { "Weight", "重量" },
    { "Anicount", "负重" },
    { "Source", "无" },
    { "Reserved", "特殊属性" },
    { "Looks", "内观" },
    { "DuraMax", "持久" },
    { "Ac", "防御下限" },
    { "AC2", "防御上限" },
    { "Mac", "魔御下限" },
    { "Mac2", "魔御上限" },
    { "Dc", "攻击下限" },
    { "Dc2", "攻击上限" },
    { "Mc", "魔法下限" },
    { "Mc2", "魔法上限" },
    { "Sc", "道术下限" },
    { "Sc2", "道术上限" },
    { "Need", "特殊条件" },
    { "NeedLevel", "需要等级" },
    { "Price", "出售价格" },
    { "Stock", "库存量" },
    { "Color", "颜色" },
    { "OverLap", "无" },
    { "HP", "血量提升" },
    { "MP", "蓝量提升" },
    { "Job", "职业" },
    { "Value1", "暴击几率增加" },
    { "Value2", "增加攻击伤害" },
    { "Value3", "物理伤害减少" },
    { "Value4", "魔法伤害减少" },
    { "Value5", "忽视目标防御" },
    { "Value6", "所有伤害反弹" },
    { "Value7", "增加目标暴率" },
    { "Value8", "人物体力增加" },
    { "Value9", "人物魔力增加" },
    { "Value10", "怒气恢复增加" },
    { "Value11", "合击攻击增加" },
    { "Value12", "Value12" },
    { "Value13", "Value13" },
    { "Value14", "Value14" },
    { "Value15", "Value15" },
    { "Value16", "Value16" },
    { "Value17", "Value17" },
    { "Value18", "Value18" },
    { "Value19", "Value19" },
    { "Value20", "Value20" },
    { "Value21", "Value21" },
    { "Value22", "Value22" },
    { "Value23", "Value23" },
    { "Value24", "Value24" },
    { "Value25", "Value25" },
    { "Value26", "Value26" },
    { "Value27", "Value27" },
    { "Value28", "Value28" },
    { "Value29", "Value29" },
    { "Value30", "Value30" },
    { "InsuranceGold", "投保金额" },
    { "InsuranceCurrency", "保金类型" },
    { "element", "暴击几率增加" },
    { "element1", "增加攻击伤害" },
    { "element2", "物理伤害减少" },
    { "element3", "魔法伤害减少" },
    { "element4", "忽视目标防御" },
    { "element5", "所有伤害反弹" },
    { "element6", "增加杀人暴率" },
    { "element7", "人物体力增加" },
    { "element8", "人物魔力增加" },
    { "element9", "怒气恢复增加" },
    { "element10", "合击攻击增加" },
    { "element11", "增加杀怪暴率" },
    { "element12", "增加防爆几率" },
    { "element13", "增加防止麻痹" },
    { "element14", "增加防止护身" },
    { "element15", "增加防止复活" },
    { "element16", "增加防止全毒" },
    { "element17", "增加防止诱惑" },
    { "element18", "增加防止火墙" },
    { "element19", "增加防止冰冻" },
    { "element20", "增加防止蛛网" },
    { "element21", "致命一击几率" },
    { "element22", "致命一击伤害增加" },
    { "element23", "致命一击防御" },
    { "element24", "暴击抗性" },
    { "element25", "攻击伤害抗性" },
    { "element26", "杀怪经验倍率" }
        }
    },{
        new int[]{ 25}, new Dictionary<string, string>
        {

            { "idx", "序号" },
    { "name", "名称" },
    { "StdMode", "分类" },
    { "Shape", "无" },
    { "Weight", "重量" },
    { "Anicount", "无" },
    { "Source", "无" },
    { "Reserved", "无" },
    { "Looks", "内观" },
    { "DuraMax", "持久" },
    { "Ac", "无" },
    { "AC2", "无" },
    { "Mac", "无" },
    { "Mac2", "无" },
    { "Dc", "无" },
    { "Dc2", "无" },
    { "Mc", "无" },
    { "Mc2", "无" },
    { "Sc", "无" },
    { "Sc2", "无" },
    { "Need", "特殊条件" },
    { "NeedLevel", "需要等级" },
    { "Price", "出售价格" },
    { "Stock", "库存量" },
    { "Color", "颜色" },
    { "OverLap", "无" },
    { "HP", "血量提升" },
    { "MP", "蓝量提升" },
    { "Job", "职业" },
    { "Value1", "暴击几率增加" },
    { "Value2", "增加攻击伤害" },
    { "Value3", "物理伤害减少" },
    { "Value4", "魔法伤害减少" },
    { "Value5", "忽视目标防御" },
    { "Value6", "所有伤害反弹" },
    { "Value7", "增加目标暴率" },
    { "Value8", "人物体力增加" },
    { "Value9", "人物魔力增加" },
    { "Value10", "怒气恢复增加" },
    { "Value11", "合击攻击增加" },
    { "Value12", "Value12" },
    { "Value13", "Value13" },
    { "Value14", "Value14" },
    { "Value15", "Value15" },
    { "Value16", "Value16" },
    { "Value17", "Value17" },
    { "Value18", "Value18" },
    { "Value19", "Value19" },
    { "Value20", "Value20" },
    { "Value21", "Value21" },
    { "Value22", "Value22" },
    { "Value23", "Value23" },
    { "Value24", "Value24" },
    { "Value25", "Value25" },
    { "Value26", "Value26" },
    { "Value27", "Value27" },
    { "Value28", "Value28" },
    { "Value29", "Value29" },
    { "Value30", "Value30" },
    { "InsuranceGold", "投保金额" },
    { "InsuranceCurrency", "保金类型" },
    { "element", "暴击几率增加" },
    { "element1", "增加攻击伤害" },
    { "element2", "物理伤害减少" },
    { "element3", "魔法伤害减少" },
    { "element4", "忽视目标防御" },
    { "element5", "所有伤害反弹" },
    { "element6", "增加杀人暴率" },
    { "element7", "人物体力增加" },
    { "element8", "人物魔力增加" },
    { "element9", "怒气恢复增加" },
    { "element10", "合击攻击增加" },
    { "element11", "增加杀怪暴率" },
    { "element12", "增加防爆几率" },
    { "element13", "增加防止麻痹" },
    { "element14", "增加防止护身" },
    { "element15", "增加防止复活" },
    { "element16", "增加防止全毒" },
    { "element17", "增加防止诱惑" },
    { "element18", "增加防止火墙" },
    { "element19", "增加防止冰冻" },
    { "element20", "增加防止蛛网" },
    { "element21", "致命一击几率" },
    { "element22", "致命一击伤害增加" },
    { "element23", "致命一击防御" },
    { "element24", "暴击抗性" },
    { "element25", "攻击伤害抗性" },
    { "element26", "杀怪经验倍率" }
        }
    },{
        new int[]{ 29}, new Dictionary<string, string>
        {

            { "idx", "序号" },
    { "name", "名称" },
    { "StdMode", "分类" },
    { "Shape", "无" },
    { "Weight", "重量" },
    { "Anicount", "触发序号" },
    { "Source", "无" },
    { "Reserved", "无" },
    { "Looks", "内观" },
    { "DuraMax", "持久" },
    { "Ac", "无" },
    { "AC2", "无" },
    { "Mac", "无" },
    { "Mac2", "无" },
    { "Dc", "无" },
    { "Dc2", "无" },
    { "Mc", "无" },
    { "Mc2", "无" },
    { "Sc", "无" },
    { "Sc2", "无" },
    { "Need", "特殊条件" },
    { "NeedLevel", "需要等级" },
    { "Price", "出售价格" },
    { "Stock", "库存量" },
    { "Color", "颜色" },
    { "OverLap", "无" },
    { "HP", "血量提升" },
    { "MP", "蓝量提升" },
    { "Job", "职业" },
    { "Value1", "暴击几率增加" },
    { "Value2", "增加攻击伤害" },
    { "Value3", "物理伤害减少" },
    { "Value4", "魔法伤害减少" },
    { "Value5", "忽视目标防御" },
    { "Value6", "所有伤害反弹" },
    { "Value7", "增加目标暴率" },
    { "Value8", "人物体力增加" },
    { "Value9", "人物魔力增加" },
    { "Value10", "怒气恢复增加" },
    { "Value11", "合击攻击增加" },
    { "Value12", "Value12" },
    { "Value13", "Value13" },
    { "Value14", "Value14" },
    { "Value15", "Value15" },
    { "Value16", "Value16" },
    { "Value17", "Value17" },
    { "Value18", "Value18" },
    { "Value19", "Value19" },
    { "Value20", "Value20" },
    { "Value21", "Value21" },
    { "Value22", "Value22" },
    { "Value23", "Value23" },
    { "Value24", "Value24" },
    { "Value25", "Value25" },
    { "Value26", "Value26" },
    { "Value27", "Value27" },
    { "Value28", "Value28" },
    { "Value29", "Value29" },
    { "Value30", "Value30" },
    { "InsuranceGold", "投保金额" },
    { "InsuranceCurrency", "保金类型" },
    { "element", "暴击几率增加" },
    { "element1", "增加攻击伤害" },
    { "element2", "物理伤害减少" },
    { "element3", "魔法伤害减少" },
    { "element4", "忽视目标防御" },
    { "element5", "所有伤害反弹" },
    { "element6", "增加杀人暴率" },
    { "element7", "人物体力增加" },
    { "element8", "人物魔力增加" },
    { "element9", "怒气恢复增加" },
    { "element10", "合击攻击增加" },
    { "element11", "增加杀怪暴率" },
    { "element12", "增加防爆几率" },
    { "element13", "增加防止麻痹" },
    { "element14", "增加防止护身" },
    { "element15", "增加防止复活" },
    { "element16", "增加防止全毒" },
    { "element17", "增加防止诱惑" },
    { "element18", "增加防止火墙" },
    { "element19", "增加防止冰冻" },
    { "element20", "增加防止蛛网" },
    { "element21", "致命一击几率" },
    { "element22", "致命一击伤害增加" },
    { "element23", "致命一击防御" },
    { "element24", "暴击抗性" },
    { "element25", "攻击伤害抗性" },
    { "element26", "杀怪经验倍率" }
        }
    },{
        new int[]{ 70}, new Dictionary<string, string>
        {

            { "idx", "序号" },
    { "name", "名称" },
    { "StdMode", "分类" },
    { "Shape", "称号编号" },
    { "Weight", "无" },
    { "Anicount", "称号显示" },
    { "Source", "无" },
    { "Reserved", "显示名称" },
    { "Looks", "内观" },
    { "DuraMax", "使用时间" },
    { "Ac", "防御下限" },
    { "AC2", "防御上限" },
    { "Mac", "魔御下限" },
    { "Mac2", "魔御上限" },
    { "Dc", "攻击下限" },
    { "Dc2", "攻击上限" },
    { "Mc", "魔法下限" },
    { "Mc2", "魔法上限" },
    { "Sc", "道术下限" },
    { "Sc2", "道术上限" },
    { "Need", "特殊条件" },
    { "NeedLevel", "需要等级" },
    { "Price", "出售价格" },
    { "Stock", "库存量" },
    { "Color", "颜色" },
    { "OverLap", "无" },
    { "HP", "血量提升" },
    { "MP", "蓝量提升" },
    { "Job", "职业" },
    { "Value1", "暴击几率增加" },
    { "Value2", "增加攻击伤害" },
    { "Value3", "物理伤害减少" },
    { "Value4", "魔法伤害减少" },
    { "Value5", "忽视目标防御" },
    { "Value6", "所有伤害反弹" },
    { "Value7", "增加目标暴率" },
    { "Value8", "人物体力增加" },
    { "Value9", "人物魔力增加" },
    { "Value10", "怒气恢复增加" },
    { "Value11", "合击攻击增加" },
    { "Value12", "Value12" },
    { "Value13", "Value13" },
    { "Value14", "Value14" },
    { "Value15", "Value15" },
    { "Value16", "Value16" },
    { "Value17", "Value17" },
    { "Value18", "Value18" },
    { "Value19", "Value19" },
    { "Value20", "Value20" },
    { "Value21", "Value21" },
    { "Value22", "Value22" },
    { "Value23", "Value23" },
    { "Value24", "Value24" },
    { "Value25", "Value25" },
    { "Value26", "Value26" },
    { "Value27", "Value27" },
    { "Value28", "Value28" },
    { "Value29", "Value29" },
    { "Value30", "Value30" },
    { "InsuranceGold", "投保金额" },
    { "InsuranceCurrency", "保金类型" },
    { "element", "暴击几率增加" },
    { "element1", "增加攻击伤害" },
    { "element2", "物理伤害减少" },
    { "element3", "魔法伤害减少" },
    { "element4", "忽视目标防御" },
    { "element5", "所有伤害反弹" },
    { "element6", "增加杀人暴率" },
    { "element7", "人物体力增加" },
    { "element8", "人物魔力增加" },
    { "element9", "怒气恢复增加" },
    { "element10", "合击攻击增加" },
    { "element11", "增加杀怪暴率" },
    { "element12", "增加防爆几率" },
    { "element13", "增加防止麻痹" },
    { "element14", "增加防止护身" },
    { "element15", "增加防止复活" },
    { "element16", "增加防止全毒" },
    { "element17", "增加防止诱惑" },
    { "element18", "增加防止火墙" },
    { "element19", "增加防止冰冻" },
    { "element20", "增加防止蛛网" },
    { "element21", "致命一击几率" },
    { "element22", "致命一击伤害增加" },
    { "element23", "致命一击防御" },
    { "element24", "暴击抗性" },
    { "element25", "攻击伤害抗性" },
    { "element26", "杀怪经验倍率" }
        }
    },
    // 添加其他语言的字典
};
        private DataGrid dataGrid;
        private Visibility _zdvisibility = Visibility.Collapsed;
        private ObservableCollection<string> _categoryItemList;
        public ObservableCollection<string> CategoryItemList
        {
            get { return _categoryItemList; }
            set
            {
                if (_categoryItemList != value)
                {
                    _categoryItemList = value;
                    RaisePropertyChanged(nameof(CategoryItemList));
                }
            }
        }
        private bool _isChinese;
        public bool IsChinese
        {
            get { return _isChinese; }
            set
            {
                _isChinese = value;
                RaisePropertyChanged(nameof(IsChinese));
                UpdateDataGridHeaders();
            }
        }
        private ExpandoObject _dgTestselectedOrder;
        public ExpandoObject dgTestSelectedOrder
        {
            get => _dgTestselectedOrder;
            set
            {
                _dgTestselectedOrder = value;
                RaisePropertyChanged(nameof(dgTestSelectedOrder));
            }
        }
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    RaisePropertyChanged(nameof(SearchText));
                }
            }
        }
        public Visibility Zdvisibility
        {
            get { return _zdvisibility; }
            set
            {
                if (_zdvisibility != value)
                {
                    _zdvisibility = value;
                    RaisePropertyChanged(nameof(Zdvisibility));
                }
            }
        }
        private ObservableCollection<Stringzd> _zdItemsSource;
        public ObservableCollection<Stringzd> ZdItemsSource
        {
            get { return _zdItemsSource; }
            set
            {
                if (_zdItemsSource != value)
                {
                    _zdItemsSource = value;
                    RaisePropertyChanged(nameof(ZdItemsSource));
                }
            }
        }
        private ObservableCollection<ExpandoObject> _originalOrders;
        private ObservableCollection<ExpandoObject> _orders;
        public ObservableCollection<ExpandoObject> Orders
        {
            get { return _orders; }
            set
            {
                if (_orders != value)
                {
                    _orders = value;
                    RaisePropertyChanged(nameof(Orders));
                }
            }
        }
        public ICommand SortCommand { get; set; }
        #region Loaded命令

        private ICommand winLoadedCommand;

        public ICommand WinLoadedCommand
        {
            get
            {
                if (winLoadedCommand == null)
                {
                    winLoadedCommand = new RelayCommand<object>(WinLoaded);
                }
                return winLoadedCommand;
            }
        }
        private RelayCommand _selectionChangedCommand = null;
        public ICommand SelectionChangedCommand => _selectionChangedCommand ??= new RelayCommand(ExecuteSelectionChangedCommand, CanExecuteSelectionChangedCommand);
        private RelayCommand _qingli1Command = null;
        public ICommand Qingli1Command => _qingli1Command ??= new RelayCommand(ExecuteQingli1Command);
        private RelayCommand _setTBTextChanged = null;
        public ICommand SetTBTextChanged => _setTBTextChanged ??= new RelayCommand(ExecuteSetTBTextChanged);

        private RelayCommand _setTypeChanged = null;
        public ICommand SetTypeChanged => _setTypeChanged ??= new RelayCommand(ExecuteSetTypeChanged);

        private RelayCommand _dgTestRowClicked = null;
        public ICommand dgTestRowClicked => _dgTestRowClicked ??= new RelayCommand(ExecutedgTestRowClicked);
        private RelayCommand _changecolumn1Clicked = null;
        public ICommand Changecolumn1Clicked => _changecolumn1Clicked ??= new RelayCommand(ExecuteChangecolumn1Clicked);
        private System.Timers.Timer _searchTimer;
        private bool isExecuting = false;
        private bool isExChinese = false;
        private async void ExecuteChangecolumn1Clicked(object parameter)
        {
            if (parameter is bool isChecked)
            {
                IsChinese = isChecked;
                isExChinese = true;
            }
        }
        Dictionary<string, string> HistorycolumnDict;
        private void UpdateDataGridHeaders()
        {
            if (dataGrid == null || dataGrid.Columns == null)
            {
                return;
            }
            if (isExChinese)
            {
                Dictionary<string, string> columnDict = null;

                var keyArray = _columnHeaders.Keys.FirstOrDefault(keyArray => keyArray.Contains(_stdmode));
                if (keyArray != null)
                {
                    if (_columnHeaders.TryGetValue(keyArray, out columnDict))
                    {
                        foreach (var column in dataGrid.Columns)
                        {
                            if (column is DataGridTextColumn textColumn && columnDict != null)
                            {
                                string originalHeader = textColumn.Header.ToString();
                                if (HistorycolumnDict != null)
                                {
                                    originalHeader = HistorycolumnDict.FirstOrDefault(entry => string.Equals(entry.Value, originalHeader, StringComparison.OrdinalIgnoreCase)).Key;
                                    if (originalHeader != null)
                                        HistorycolumnDict.Remove(originalHeader);
                                }

                                string newHeader = IsChinese
        ? (columnDict.Keys.FirstOrDefault(key => string.Equals(key, originalHeader, StringComparison.OrdinalIgnoreCase)) is string key && key != null ? columnDict[key] : originalHeader)
        : (columnDict.FirstOrDefault(entry => string.Equals(entry.Value, originalHeader, StringComparison.OrdinalIgnoreCase)).Key ?? originalHeader);
                                textColumn.Header = newHeader;
                            }
                        }
                    }
                }
                isExChinese=false;
            }
            else
            {
                Dictionary<string, string> columnDict = null;

                var keyArray = _columnHeaders.Keys.FirstOrDefault(keyArray => keyArray.Contains(_stdmode));
                if (keyArray != null)
                {
                    if (_columnHeaders.TryGetValue(keyArray, out columnDict))
                    {
                        foreach (var column in dataGrid.Columns)
                        {
                            if (column is DataGridTextColumn textColumn && columnDict != null)
                            {
                                string originalHeader = textColumn.Header.ToString();
                                if (HistorycolumnDict != null)
                                {
                                    originalHeader = HistorycolumnDict.FirstOrDefault(entry => string.Equals(entry.Value, originalHeader, StringComparison.OrdinalIgnoreCase)).Key;
                                    if (originalHeader != null)
                                        HistorycolumnDict.Remove(originalHeader);
                                }

                                string newHeader = IsChinese
        ? (columnDict.Keys.FirstOrDefault(key => string.Equals(key, originalHeader, StringComparison.OrdinalIgnoreCase)) is string key && key != null ? columnDict[key] : originalHeader)
        : (columnDict.FirstOrDefault(entry => string.Equals(entry.Value, originalHeader, StringComparison.OrdinalIgnoreCase)).Key ?? originalHeader);
                                textColumn.Header = newHeader;
                            }
                        }
                    }
                }
                HistorycolumnDict = new Dictionary<string, string>(columnDict, StringComparer.OrdinalIgnoreCase);
            }
        }
        private int _stdmode = 5;
        private async void ExecutedgTestRowClicked(object parameter)
        {
            CategoryItemList = new ObservableCollection<string>();
            // 这里parameter就是你通过CommandParameter传递的SelectedCells
            var selectedCells = parameter as IList<DataGridCellInfo>;
            if (selectedCells != null && selectedCells.Count > 0)
            {
                // 获取最后一个选中的单元格
                var lastCell = selectedCells[selectedCells.Count - 1];

                // 处理最后一个选中的单元格
                var cellValue = lastCell.Item; // 这取决于你的DataGrid绑定的数据类型
                string dc = string.Empty;
                string mc = string.Empty;
                string sc = string.Empty;
                string ac = string.Empty;
                string mac = string.Empty;
                string stdmode = string.Empty;
                
                string shape = string.Empty;
                foreach (var item in (ExpandoObject)cellValue)
                {
                    switch (item.Key.ToLower())
                    {
                        case "idx":
                            CategoryItemList.Add("物品编号：" + item.Value);
                            break;
                        case "name":
                            CategoryItemList.Add("物品名称：" + item.Value);
                            break;
                        case "stdmode":
                            stdmode = item.Value.ToString();
                            _stdmode = int.Parse(stdmode);
                            if (stdmode == "25"|| stdmode == "2")
                            { 
                            
                            }
                            else
                            {
                                CategoryItemList.Add("物品类型：" + Getstdmode(int.Parse(item.Value.ToString())));
                            }
                            break;
                        case "shape":
                            shape=item.Value.ToString();
                            if (stdmode == "25")
                            {
                                if (shape == "1")
                                {
                                    CategoryItemList.Add("物品类型：" + "绿毒(持续掉血)");
                                }
                                else if (shape == "2")
                                {
                                    CategoryItemList.Add("物品类型：" + "黄毒(降低防御)");
                                }
                                else if (shape == "3")
                                {
                                    CategoryItemList.Add("物品类型：" + "蓝毒(死亡之眼)");
                                }
                                else if (shape == "5")
                                {
                                    CategoryItemList.Add("物品类型：" + "符(灵魂火符)");
                                }
                                else
                                {
                                    CategoryItemList.Add("物品类型：" + "毒符");
                                }
                            }
                            if (stdmode == "2")
                            {
                                if (shape == "0")
                                {
                                    CategoryItemList.Add("物品类型：" + "千里传音");
                                }
                                else if(shape == "1")
                                {
                                    CategoryItemList.Add("物品类型：" + "自定义计次物品");
                                }
                                else if (shape == "2"|| shape == "3")
                                {
                                    CategoryItemList.Add("物品类型：" + "随机传送");
                                }
                                else if (shape == "9")
                                {
                                    CategoryItemList.Add("物品类型：" + "修复装备");
                                }
                            }
                            if (stdmode == "0")
                            {
                                if (shape == "0")
                                {
                                    CategoryItemList.Add("恢复类型：" + "缓慢恢复");
                                }
                                else if (shape == "1")
                                {
                                    CategoryItemList.Add("恢复类型：" + "快速恢复");
                                }
                            }
                            break;
                        case "looks":
                            CategoryItemList.Add("物品内观：" + item.Value);
                            break;
                        case "weight":
                            CategoryItemList.Add("物品重量：" + item.Value);
                            break;
                        case "duramax":
                            if (stdmode == "2")
                            {
                                if (shape == "9")
                                {
                                    CategoryItemList.Add("可用次数：" + int.Parse(item.Value.ToString()) / 100);
                                }
                                else
                                {
                                    CategoryItemList.Add("可用次数：" + int.Parse(item.Value.ToString()) / 1000);
                                }
                            }
                            else if (stdmode == "49")
                            {
                                CategoryItemList.Add("聚灵珠经验：" + item.Value.ToString()+"W");
                            }
                            else
                            {
                                CategoryItemList.Add("物品持久：" + int.Parse(item.Value.ToString()) / 1000);
                            }  
                            break;
                        case "anicount":
                            if (stdmode == "70" || stdmode == "71" || stdmode == "72" || stdmode == "73" || stdmode == "74")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("叠加属性：" + "叠加");
                                else
                                    CategoryItemList.Add("叠加属性：" + "不叠加");
                            }
                            else if (stdmode == "31")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("触发脚本：" + "[@StdModeFunc"+ item.Value + "]");
                            }
                            break;
                        case "reserved":
                            if (stdmode == "70" || stdmode == "71" || stdmode == "72" || stdmode == "73" || stdmode == "74")
                            {
                                if (Convert.ToInt32(item.Value) == 0)
                                    CategoryItemList.Add("显示方式：" + "名字+图标");
                                else if (Convert.ToInt32(item.Value) == 1)
                                    CategoryItemList.Add("显示方式：" + "只显示图标");
                                else if (Convert.ToInt32(item.Value) == 2)
                                    CategoryItemList.Add("显示方式：" + "不显示");
                            }
                            break;
                        case "ac":
                            if (stdmode == "5" || stdmode == "6" || stdmode == "68" || stdmode == "69")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("幸　　运：" + item.Value);
                            }
                            else if (stdmode == "15" || stdmode == "78" || stdmode == "26" || stdmode == "80" || stdmode == "22" || stdmode == "81" || stdmode == "16" || stdmode == "10" || stdmode == "66" || stdmode == "67" || stdmode == "11" || stdmode == "30" || stdmode == "83" || stdmode == "62" || stdmode == "63" || stdmode == "64" || stdmode == "84" || stdmode == "85" || stdmode == "86" || stdmode == "87" || stdmode == "88" || stdmode == "89" || stdmode == "28" || stdmode == "12" || stdmode == "65" || stdmode == "90" || stdmode == "73")
                            {
                                ac = item.Value.ToString() + "-";
                            }
                            else if (stdmode == "72")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("攻击速度+：" + item.Value);
                            }
                            else if (stdmode == "74")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("攻击速度：" + item.Value);
                            }
                            else if (stdmode == "0")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("恢复 HP：" + item.Value);
                            }
                            break;
                        case "ac2":
                            if (stdmode == "5" || stdmode == "6" || stdmode == "68" || stdmode == "69")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("准　　确：" + item.Value);
                            }
                            else if (stdmode == "15" || stdmode == "78" || stdmode == "26" || stdmode == "80" || stdmode == "22" || stdmode == "81" || stdmode == "16" || stdmode == "10" || stdmode == "66" || stdmode == "67" || stdmode == "11" || stdmode == "30" || stdmode == "83" || stdmode == "62" || stdmode == "63" || stdmode == "64" || stdmode == "84" || stdmode == "85" || stdmode == "86" || stdmode == "87" || stdmode == "88" || stdmode == "89" || stdmode == "28" || stdmode == "12" || stdmode == "65" || stdmode == "90" || stdmode == "73")
                            {
                                CategoryItemList.Add("防　　御：" + ac + item.Value.ToString());
                            }
                            else if (stdmode == "19" || stdmode == "75" || stdmode == "70")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("魔法躲避：" + int.Parse(item.Value.ToString())*10+"%");
                            }
                            else if (stdmode == "20" || stdmode == "76" || stdmode == "24" || stdmode == "79" || stdmode == "71")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("准　　确：" + item.Value);
                            }
                            else if (stdmode == "21" || stdmode == "77" || stdmode == "72")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("体力恢复：" + int.Parse(item.Value.ToString()) * 10 + "%");
                            }
                            else if (stdmode == "23" || stdmode == "82" || stdmode == "74")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("毒物躲避：" + int.Parse(item.Value.ToString()) * 10 + "%");
                            }
                            break;
                        case "mac":
                            if (stdmode == "5" || stdmode == "6" || stdmode == "70" || stdmode == "68" || stdmode == "69")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("诅　　咒：" + item.Value);
                            }
                            else if (stdmode == "15" || stdmode == "78" || stdmode == "26" || stdmode == "80" || stdmode == "22" || stdmode == "81" || stdmode == "16" || stdmode == "10" || stdmode == "66" || stdmode == "67" || stdmode == "11" || stdmode == "30" || stdmode == "83" || stdmode == "62" || stdmode == "63" || stdmode == "64" || stdmode == "84" || stdmode == "85" || stdmode == "86" || stdmode == "87" || stdmode == "88" || stdmode == "89" || stdmode == "28" || stdmode == "12" || stdmode == "65" || stdmode == "90" || stdmode == "73")
                            {
                                mac = item.Value.ToString() + "-";
                            }
                            else if (stdmode == "72")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("攻击速度-：" + item.Value);
                            }
                            else if (stdmode == "0")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("恢复 MP：" + item.Value);
                            }
                            break;
                        case "mac2":
                            if (stdmode == "5" || stdmode == "6" || stdmode == "68" || stdmode == "69")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("攻　　速：" + item.Value);
                            }
                            else if (stdmode == "15" || stdmode == "78" || stdmode == "26" || stdmode == "80" || stdmode == "22" || stdmode == "81" || stdmode == "16" || stdmode == "10" || stdmode == "66" || stdmode == "67" || stdmode == "11" || stdmode == "30" || stdmode == "83" || stdmode == "62" || stdmode == "63" || stdmode == "64" || stdmode == "84" || stdmode == "85" || stdmode == "86" || stdmode == "87" || stdmode == "88" || stdmode == "89" || stdmode == "28" || stdmode == "12" || stdmode == "65" || stdmode == "90" || stdmode == "73")
                            {
                                CategoryItemList.Add("魔　　御：" + mac + item.Value.ToString());
                            }
                            else if (stdmode == "19" || stdmode == "75" || stdmode == "70")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("幸　　运：" + item.Value);
                            }
                            else if (stdmode == "20" || stdmode == "76" || stdmode == "24" || stdmode == "79" || stdmode == "71")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("敏　　捷：" + item.Value);
                            }
                            else if (stdmode == "21" || stdmode == "77" || stdmode == "72")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("魔法恢复：" + int.Parse(item.Value.ToString()) * 10 + "%");
                            }
                            else if (stdmode == "23" || stdmode == "82" || stdmode == "73")
                            {
                                if (Convert.ToInt32(item.Value) > 0)
                                    CategoryItemList.Add("中毒恢复：" + int.Parse(item.Value.ToString()) * 10 + "%");
                            }
                            break;
                        case "dc":
                            if (stdmode == "5" || stdmode == "6" || stdmode == "10" || stdmode == "66" || stdmode == "67" || stdmode == "11" || stdmode == "12" || stdmode == "15" || stdmode == "78" || stdmode == "16" || stdmode == "19" || stdmode == "75" || stdmode == "20" || stdmode == "76" || stdmode == "21" || stdmode == "77" || stdmode == "22" || stdmode == "81" || stdmode == "23" || stdmode == "82" || stdmode == "24" || stdmode == "79" || stdmode == "26" || stdmode == "80" || stdmode == "28" || stdmode == "29" || stdmode == "30" || stdmode == "83" || stdmode == "62" || stdmode == "63" || stdmode == "64" || stdmode == "84" || stdmode == "85" || stdmode == "86" || stdmode == "87" || stdmode == "88" || stdmode == "89" || stdmode == "65" ||  stdmode == "90" || stdmode == "70" || stdmode == "71" || stdmode == "72" || stdmode == "73" || stdmode == "74" || stdmode == "68" || stdmode == "69")
                                dc = item.Value.ToString() + "-";
                            break;
                        case "dc2":
                            if (stdmode == "5" || stdmode == "6" || stdmode == "10" || stdmode == "66" || stdmode == "67" || stdmode == "11" || stdmode == "12" || stdmode == "15" || stdmode == "78" || stdmode == "16" || stdmode == "19" || stdmode == "75" || stdmode == "20" || stdmode == "76" || stdmode == "21" || stdmode == "77" || stdmode == "22" || stdmode == "81" || stdmode == "23" || stdmode == "82" || stdmode == "24" || stdmode == "79" || stdmode == "26" || stdmode == "80" || stdmode == "28" || stdmode == "29" || stdmode == "30" || stdmode == "83" || stdmode == "62" || stdmode == "63" || stdmode == "64" || stdmode == "84" || stdmode == "85" || stdmode == "86" || stdmode == "87" || stdmode == "88" || stdmode == "89" || stdmode == "65" || stdmode == "90" || stdmode == "70" || stdmode == "71" || stdmode == "72" || stdmode == "73" || stdmode == "74" || stdmode == "68" || stdmode == "69")
                                CategoryItemList.Add("攻　　击：" + dc + item.Value.ToString());
                            break;
                        case "mc":
                            if (stdmode == "5" || stdmode == "6" || stdmode == "10" || stdmode == "66" || stdmode == "67" || stdmode == "11" || stdmode == "12" || stdmode == "15" || stdmode == "78" || stdmode == "16" || stdmode == "19" || stdmode == "75" || stdmode == "20" || stdmode == "76" || stdmode == "21" || stdmode == "77" || stdmode == "22" || stdmode == "81" || stdmode == "23" || stdmode == "82" || stdmode == "24" || stdmode == "79" || stdmode == "26" || stdmode == "80" || stdmode == "28" || stdmode == "29" || stdmode == "30" || stdmode == "83" || stdmode == "62" || stdmode == "63" || stdmode == "64" || stdmode == "84" || stdmode == "85" || stdmode == "86" || stdmode == "87" || stdmode == "88" || stdmode == "89" || stdmode == "65" || stdmode == "90" || stdmode == "70" || stdmode == "71" || stdmode == "72" || stdmode == "73" || stdmode == "74" || stdmode == "68" || stdmode == "69")
                                mc = item.Value.ToString() + "-";
                            break;
                        case "mc2":
                            if (stdmode == "5" || stdmode == "6" || stdmode == "10" || stdmode == "66" || stdmode == "67" || stdmode == "11" || stdmode == "12" || stdmode == "15" || stdmode == "78" || stdmode == "16" || stdmode == "19" || stdmode == "75" || stdmode == "20" || stdmode == "76" || stdmode == "21" || stdmode == "77" || stdmode == "22" || stdmode == "81" || stdmode == "23" || stdmode == "82" || stdmode == "24" || stdmode == "79" || stdmode == "26" || stdmode == "80" || stdmode == "28" || stdmode == "29" || stdmode == "30" || stdmode == "83" || stdmode == "62" || stdmode == "63" || stdmode == "64" || stdmode == "84" || stdmode == "85" || stdmode == "86" || stdmode == "87" || stdmode == "88" || stdmode == "89" || stdmode == "65" || stdmode == "90" || stdmode == "70" || stdmode == "71" || stdmode == "72" || stdmode == "73" || stdmode == "74" || stdmode == "68" || stdmode == "69")
                                CategoryItemList.Add("魔　　法：" + mc + item.Value.ToString());
                            break;
                        case "sc":
                            if (stdmode == "5" || stdmode == "6" || stdmode == "10" || stdmode == "66" || stdmode == "67" || stdmode == "11" || stdmode == "12" || stdmode == "15" || stdmode == "78" || stdmode == "16" || stdmode == "19" || stdmode == "75" || stdmode == "20" || stdmode == "76" || stdmode == "21" || stdmode == "77" || stdmode == "22" || stdmode == "81" || stdmode == "23" || stdmode == "82" || stdmode == "24" || stdmode == "79" || stdmode == "26" || stdmode == "80" || stdmode == "28" || stdmode == "29" || stdmode == "30" || stdmode == "83" || stdmode == "62" || stdmode == "63" || stdmode == "64" || stdmode == "84" || stdmode == "85" || stdmode == "86" || stdmode == "87" || stdmode == "88" || stdmode == "89" || stdmode == "65" || stdmode == "90" || stdmode == "70" || stdmode == "71" || stdmode == "72" || stdmode == "73" || stdmode == "74" || stdmode == "68" || stdmode == "69")
                                sc = item.Value.ToString() + "-";
                            break;
                        case "sc2":
                            if (stdmode == "5" || stdmode == "6" || stdmode == "10" || stdmode == "66" || stdmode == "67" || stdmode == "11" || stdmode == "12" || stdmode == "15" || stdmode == "78" || stdmode == "16" || stdmode == "19" || stdmode == "75" || stdmode == "20" || stdmode == "76" || stdmode == "21" || stdmode == "77" || stdmode == "22" || stdmode == "81" || stdmode == "23" || stdmode == "82" || stdmode == "24" || stdmode == "79" || stdmode == "26" || stdmode == "80" || stdmode == "28" || stdmode == "29" || stdmode == "30" || stdmode == "83" || stdmode == "62" || stdmode == "63" || stdmode == "64" || stdmode == "84" || stdmode == "85" || stdmode == "86" || stdmode == "87" || stdmode == "88" || stdmode == "89" || stdmode == "65" || stdmode == "90" || stdmode == "70" || stdmode == "71" || stdmode == "72" || stdmode == "73" || stdmode == "74" || stdmode == "68" || stdmode == "69")
                                CategoryItemList.Add("道　　术：" + sc + item.Value.ToString());
                            break;
                        case "color":
                            CategoryItemList.Add("物品颜色：" + item.Value);
                            break;
                        case "hp":
                            if (Convert.ToInt32(item.Value) > 0)
                            { 
                                CategoryItemList.Add("HP+　　：" + item.Value);
                            }
                            break;
                        case "mp":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("MP+　　：" + item.Value);
                            }
                            break;
                        case "element":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("暴击几率：" + item.Value);
                            }
                            break;
                        case "value1":
                        case "element1":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("攻击伤害：" + item.Value + "%");
                            }
                            break;
                        case "value2":
                        case "element2":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("伤害吸收：" + item.Value + "%");
                            }
                            break;
                        case "value3":
                        case "element3":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("魔法防御：" + item.Value + "%");
                            }
                            break;
                        case "value4":
                        case "element4":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("忽视防御：" + item.Value + "%");
                            }
                            break;
                        case "value5":
                        case "element5":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("伤害反弹：" + item.Value + "%");
                            }
                            break;
                        case "value6":
                        case "element6":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("人物暴率：" + item.Value + "%");
                            }
                            break;
                        case "value7":
                        case "element7":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("体力增加：" + item.Value + "%");
                            }
                            break;
                        case "value8":
                        case "element8":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("魔力增加：" + item.Value + "%");
                            }
                            break;
                        case "value9":
                        case "element9":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("怒气恢复：" + item.Value + "%");
                            }
                            break;
                        case "value10":
                        case "element10":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("合击伤害：" + item.Value + "%");
                            }
                            break;
                        case "element11":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("怪物暴率：" + item.Value + "%");
                            }
                            break;
                        case "element12":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("防暴几率：" + item.Value + "%");
                            }
                            break;
                        case "element13":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("防止麻痹：" + item.Value + "%");
                            }
                            break;
                        case "element14":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("防止护身：" + item.Value + "%");
                            }
                            break;
                        case "element15":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("防止复活：" + item.Value + "%");
                            }
                            break;
                        case "element16":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("防止全毒：" + item.Value + "%");
                            }
                            break;
                        case "element17":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("防止诱惑：" + item.Value + "%");
                            }
                            break;
                        case "element18":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("防止火墙：" + item.Value + "%");
                            }
                            break;
                        case "element19":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("防止冰冻：" + item.Value + "%");
                            }
                            break;
                        case "element20":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("防止蛛网：" + item.Value + "%");
                            }
                            break;
                        case "element21":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("致命一击几率：" + item.Value + "%");
                            }
                            break;
                        case "element22":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("致命一击伤害增加：" + item.Value + "%");
                            }
                            break;
                        case "element23":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("致命一击防御：" + item.Value + "%");
                            }
                            break;
                        case "element24":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("暴击抗性：" + item.Value + "%");
                            }
                            break;
                        case "element25":
                            if (Convert.ToInt32(item.Value) > 0)
                            {
                                CategoryItemList.Add("攻击伤害抗性：" + item.Value + "%");
                            }
                            break;
                    }
                }
            }
            UpdateDataGridHeaders();
        }
        private string Getstdmode(int stdmode)
        {
            switch (stdmode)
            {
                case 4:
                    return "技能书";
                case 5:
                    return "单手武器";
                case 6:
                    return "双手武器";
                case 15:
                    return "头盔";
                case 16:
                    return "斗笠面巾";
                case 10:
                    return "男衣服";
                case 11:
                    return "女衣服";
                case 30:
                    return "勋章蜡烛";
                case 19:
                case 20:
                case 21:
                    return "项链";
                case 24:
                case 26:
                    return "手镯";
                case 22:
                case 23:
                    return "戒指";
                case 64:
                    return "腰带";
                case 62:
                    return "靴子";
                case 63:
                    return "石头";
                case 28:
                    return "马牌";
                case 12:
                    return "盾牌";
                case 65:
                    return "军鼓";
                case 25:
                    return "毒符";
                case 29:
                    return "翅膀";
                case 70:
                case 71:
                case 72:
                case 73:
                case 74:
                    return "称号";
                case 53:
                    return "气血石";
                case 90:
                    return "灵玉";
                case 31:
                    return "双击触发物品";
                case 0:
                    return "药品";
                case 3:
                    return "卷轴";
                case 1:
                    return "食物";
                case 46:
                case 55:
                case 56:
                case 57:
                case 58:
                case 59:
                    return "镶嵌宝石";
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                    return "杂物";
                case 49:
                    return "聚灵珠";
                case 68:
                    return "单手时装武器";
                case 69:
                    return "双手时装武器";
                case 66:
                    return "时装(男)";
                case 67:
                    return "时装(女)";
                case 78:
                    return "时装头盔";
                case 83:
                    return "时装勋章";
                case 75:
                case 76:
                case 77:
                    return "时装项链";
                case 79:
                case 80:
                    return "时装手镯";
                case 81:
                case 82:
                    return "时装戒指";
                case 84:
                case 85:
                    return "时装腰带";
                case 86:
                case 87:
                    return "时装靴子";
                case 88:
                case 89:
                    return "时装宝石";
                default:
                    return "未知";
            }
        }
        private async void ExecuteSetTBTextChanged(object parameter)
        {
            bangdingziduan = null;
            if (_zdvisibility != Visibility.Collapsed)
            {
                bangdingziduan = (parameter as Stringzd).Name;
            }
            if (bangdingziduan != null)
            {
                await PerformSearch(bangdingziduan, bangdingvalue);
            }
            else
            {
                await PerformSearch(null, bangdingvalue);
            }
            // 停止并重置定时器
            //_searchTimer?.Stop();
            //_searchTimer?.Dispose();
            //_searchTimer = new System.Timers.Timer(500);
            //_searchTimer.Elapsed += async (sender, e) =>
            //{
            //    if (!isExecuting)
            //    {
            //        isExecuting = true;
            //        if (bangdingziduan != null)
            //        {
            //            await PerformSearch(bangdingziduan, bangdingvalue);
            //        }
            //        else
            //        {
            //            await PerformSearch(null, bangdingvalue);
            //        }
            //        isExecuting = false;
            //    }
            //        //await Task.Delay(300); // 添加额外的延迟
            //};
            //_searchTimer.Start();
        }
        private string bangdingziduan=null;
        private List<string> bangdingvalue = new List<string>();
        private async void ExecuteSetTypeChanged(object parameter)
        { 
            var p =parameter as CheckBoxParameters;
            string[] value = null;
            value=p.Tag.Split(',');
            if (p.IsChecked)
            {
                foreach (var item in value)
                {
                    bangdingvalue.Add(item);
                }
            }
            else
            {
                foreach (var item in value)
                {
                    bangdingvalue.Remove(item);
                }
            }

            if (bangdingziduan != null)
            {
                await PerformSearch(bangdingziduan, bangdingvalue);
            }
            else
            {
                await PerformSearch(null, bangdingvalue);
            }

            
        }
        private void ExecuteSelectionChangedCommand(object parameter)
        {
            // 处理选择改变后的逻辑
            int p = int.Parse(parameter.ToString());
            if (p == 0)
                Zdvisibility = System.Windows.Visibility.Collapsed;
            else
                Zdvisibility = Visibility.Visible;
        }
        private void ExecuteQingli1Command(object parameter)
        {
            SearchText = string.Empty;
        }
        private bool CanExecuteSelectionChangedCommand(object parameter)
        {
            return true;
        }
        private async void WinLoaded(object sender)
        {
            if (sender != null)
            {
                var control = sender as Sql;
                if (control != null)
                {
                    this.dataGrid = control.dgTest;
                }
            }
            if (Orders == null || Orders.Count < 1)
            {
                await LoadDataAsync();
            }
        }
        private async Task PerformSearch(string name, List<string> value)
        {
            var filteredOrders = new ObservableCollection<ExpandoObject>();
            if (value == null || value.Count==0)
            {
                if (string.IsNullOrEmpty(name))
                {
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        foreach (var order in _originalOrders)
                        {
                            var dictionary = order as IDictionary<string, object>;
                            if (dictionary != null && dictionary.Any(kvp => kvp.Value != null && kvp.Value.ToString().Contains(SearchText)))
                            {
                                filteredOrders.Add(order);
                            }
                        }
                    }
                    else
                    {
                        filteredOrders = _originalOrders;
                    }
                }
                else
                {
                    foreach (var order in _originalOrders)
                    {
                        var dictionary = order as IDictionary<string, object>;
                        if (dictionary != null && dictionary.ContainsKey(name) && dictionary[name] != null && dictionary[name].ToString().Contains(SearchText))
                        {
                            filteredOrders.Add(order);
                        }
                    }
                }
            }
            else
            {
                string[] values = value.AsReadOnly().Cast<string>().ToArray();
                if (string.IsNullOrEmpty(name))
                {
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        foreach (var order in _originalOrders)
                        {
                            var dictionary = order as IDictionary<string, object>;
                            if (dictionary != null)
                            {
                                string stdModeValue = dictionary.Keys.Contains("StdMode") ? dictionary["StdMode"]?.ToString() : null;
                                if (stdModeValue != null)
                                {
                                    stdModeValue = stdModeValue.ToUpper(); // 或者 ToLower()，根据你的需求统一为大写或小写
                                }
                                if (dictionary.Any(kvp => kvp.Value != null && kvp.Value.ToString().Contains(SearchText) && (values.Contains(stdModeValue))))
                                {
                                    filteredOrders.Add(order);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var order in _originalOrders)
                        {
                            var dictionary = order as IDictionary<string, object>;
                            if (dictionary != null)
                            {
                                string stdModeValue = dictionary.Keys.Contains("StdMode") ? dictionary["StdMode"]?.ToString() : null;
                                if (stdModeValue != null)
                                {
                                    stdModeValue = stdModeValue.ToUpper(); // 或者 ToLower()，根据你的需求统一为大写或小写
                                }
                                if (values.Contains(stdModeValue))
                                {
                                    filteredOrders.Add(order);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var order in _originalOrders)
                    {
                        var dictionary = order as IDictionary<string, object>;
                        if (dictionary != null && dictionary.ContainsKey(name) && dictionary[name] != null && dictionary[name].ToString().Contains(SearchText))
                        {
                            string stdModeValue = dictionary.Keys.Contains("StdMode") ? dictionary["StdMode"]?.ToString() : null;
                            if (stdModeValue != null)
                            {
                                stdModeValue = stdModeValue.ToUpper(); // 或者 ToLower()，根据你的需求统一为大写或小写
                            }
                            if (values.Contains(stdModeValue))
                            {
                                filteredOrders.Add(order);
                            }
                        }
                    }
                }
            }
            Orders = filteredOrders;
        }
        private async Task LoadDataAsync()
        {
            var dataTable = await Task.Run(() => db.Queryable<object>().AS("StdItems", "o").ToDataTable());
            List<Stringzd> myData = new List<Stringzd>();
            foreach (DataRow row in dataTable.Rows)
            {
                dynamic item = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)item;
                int a = 0;
                foreach (DataColumn column in dataTable.Columns)
                {
                    if (a < 100)
                    {
                        dictionary[column.ColumnName] = row[column];
                        
                    }
                    else
                        break;
                    a++;
                }
                this.Orders.Add(item);
            }
            if (dataTable.Rows.Count > 0)
            {
                int a = 0;
                foreach (DataColumn column in dataTable.Columns)
                {
                    if (a < 100)
                    {
                        int minWidth;
                        if (a == 0)
                            minWidth = 30;
                        else if (a == 1)
                            minWidth = 150;
                        else
                            minWidth = 60;
                        var dataGridTextColumn = new DataGridTextColumn()
                        {
                            Header = column.ColumnName,
                            MinWidth = minWidth,
                            Binding = new System.Windows.Data.Binding(column.ColumnName),
                            SortMemberPath = column.ColumnName
                        };
                       
                        this.dataGrid.Columns.Add(dataGridTextColumn);
                        myData.Add(new Stringzd() { Name= column.ColumnName });
                    }
                    else
                        break;
                    a++;
                }
            }
            ZdItemsSource = new ObservableCollection<Stringzd>(myData);
            SortCommand = new RelayCommand<string>(SortCommandExecute);
            _originalOrders = new ObservableCollection<ExpandoObject>();
            foreach (var order in _orders)
            {
                var newOrder = new ExpandoObject();
                var dictionary = order as IDictionary<string, object>;
                if (dictionary != null)
                {
                    foreach (var kvp in dictionary)
                    {
                        ((IDictionary<string, object>)newOrder).Add(kvp.Key, kvp.Value);
                    }
                }
                _originalOrders.Add(newOrder);
            }
        }
        private async void SortCommandExecute(string propertyName)
        {
            await Task.Delay(100);

            var sortedOrders = Orders.OrderBy(o => ((IDictionary<string, object>)o)[propertyName]).ToList();
            Orders = new ObservableCollection<ExpandoObject>(sortedOrders);
            

        }
        #endregion
        public SqlViewModel()
        {
            _orders = new ObservableCollection<ExpandoObject>();
            _zdItemsSource = new ObservableCollection<Stringzd>();
            _originalOrders = new ObservableCollection<ExpandoObject>(_orders);


        }
        private void LoadData()
        {
            var dataTable = db.Queryable<object>().AS("StdItems", "o").ToDataTable();
            Orders = new ObservableCollection<ExpandoObject>((IEnumerable<ExpandoObject>)dataTable);//Convert(dataTable);
            // 动态生成列
        }
        public static string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + @"D:\HeroDB.MDB";
        public SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            DbType = SqlSugar.DbType.Access,
            ConnectionString = connectionString,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute
        });
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class Stringzd
    {
        public string Name { get; set; }
    }
}
