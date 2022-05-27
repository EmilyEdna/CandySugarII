using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CandySugar.Library.Template
{
    public class CandyTextBox : TextBox
    {
        private char PassWordChar = '●';
        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public string PlaceHolder
        {
            get { return (string)GetValue(PlaceHolderProperty); }
            set { SetValue(PlaceHolderProperty, value); }
        }
        public string PassWord
        {
            get { return (string)GetValue(PassWordProperty); }
            set { SetValue(PassWordProperty, value); }
        }
        public bool IsPassWord
        {
            get { return (bool)GetValue(IsPassWordProperty); }
            set { SetValue(IsPassWordProperty, value); }
        }
        public bool ShowDropList
        {
            get { return (bool)GetValue(ShowDropListProperty); }
            set { SetValue(ShowDropListProperty, value); }
        }
        public string SelectType
        {
            get { return (string)GetValue(SelectTypeProperty); }
            set { SetValue(SelectTypeProperty, value); }
        }
        public IEnumerable Source
        {
            get { return (IEnumerable)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SelectTypeProperty =
            DependencyProperty.Register("SelectType", typeof(string), typeof(CandyTextBox), new PropertyMetadata(""));
        public static readonly DependencyProperty ShowDropListProperty =
            DependencyProperty.Register("ShowDropList", typeof(bool), typeof(CandyTextBox), new PropertyMetadata(false));
        public static readonly DependencyProperty IsPassWordProperty =
            DependencyProperty.Register("IsPassWord", typeof(bool), typeof(CandyTextBox), new PropertyMetadata(false));
        public static readonly DependencyProperty PassWordProperty =
            DependencyProperty.Register("PassWord", typeof(string), typeof(CandyTextBox), new PropertyMetadata(""));
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(CandyTextBox), new PropertyMetadata(null));
        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty.Register("PlaceHolder", typeof(string), typeof(CandyTextBox), new PropertyMetadata(""));
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(IEnumerable), typeof(CandyTextBox), new PropertyMetadata(null));

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (IsPassWord)
            {
                if (PassWord == null)
                    PassWord = string.Empty;
                //已键入的文本长度
                int textLength = Text.Length;
                //已保存的密码长度
                int psdLength = PassWord.Length;
                //起始修改位置
                int startIndex = -1;
                for (int i = 0; i < textLength; i++)
                {
                    if (Text[i] != PassWordChar)
                    {
                        startIndex = i;
                        break;
                    }
                }
                //未作任何修改
                if (startIndex == -1 && textLength == psdLength) return;
                //结束修改位置
                int stopIndex = -1;
                for (int i = textLength - 1; i >= 0; i--)
                {
                    if (Text[i] != PassWordChar)
                    {
                        stopIndex = i;
                        break;
                    }
                }
                //添加或修改了一个或连续的多个值
                if (startIndex != -1)
                {
                    //累计修改长度
                    int alterLength = stopIndex - startIndex + 1;
                    //长度没变化，单纯的修改了一个或连续的多个值
                    if (textLength == psdLength)
                    {
                        PassWord = PassWord.Substring(0, startIndex) + Text.Substring(startIndex, alterLength) + PassWord.Substring(stopIndex + 1);
                    }
                    //新增了内容
                    else
                    {
                        //新增且修改了原来内容
                        if (alterLength > textLength - psdLength)
                        {
                            //计算未修改密码个数 textLength - alterLength
                            //计算已修改密码个数 = 原密码长度 - 未修改密码个数 psdLength - (textLength - alterLength)
                            //原密码该保留的后半部分的索引 = 已修改密码个数 + 起始修改位置
                            PassWord = PassWord.Substring(0, startIndex) + Text.Substring(startIndex, alterLength) + PassWord.Substring(psdLength - (textLength - alterLength) + startIndex);
                        }
                        //单纯的新增了一个或多个连续的值
                        else
                        {
                            PassWord = PassWord.Substring(0, startIndex) + Text.Substring(startIndex, alterLength) + PassWord.Substring(startIndex);
                        }

                    }
                }
                //删除了一个或连续的多个值
                else
                {
                    //已删除的数据长度
                    int length = psdLength - textLength;
                    if (SelectionStart < textLength)
                    {
                        //改变了中间的数据
                        PassWord = PassWord.Substring(0, SelectionStart) + PassWord.Substring(SelectionStart + length);
                    }
                    else
                    {
                        //删除了结尾的数据
                        PassWord = PassWord.Substring(0, SelectionStart);
                    }
                }
                //记住光标位置(设置完Text后会丢失，所以现在要记住)
                int selectionStart = SelectionStart;
                //设置显示密码
                Text = new string(PassWordChar, textLength);
                //设置光标位置
                SelectionStart = selectionStart;
            }
        }
    }
}
