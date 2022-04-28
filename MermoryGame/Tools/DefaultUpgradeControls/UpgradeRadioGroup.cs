using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MermoryGame.Control;

namespace MermoryGame.View.UpgradeControls
{
    public class UpgradeRadioGroup : Panel, Moving_Resizing //Класс области с выбором одного элемента из нескольких
    {
        public List<RadioButton>
            Elements = new List<RadioButton>(); //Список элментов с галочкой, где для элментов лежащих в

        public UpgradeLabel RadioLabel; //Переменная хранящая надпись

        public UpgradePanel RadioPanel; //Переменная хранящая панель в которой будут расположены варианты ответа
        //одном родителе, может быть отмечена лишь одна галочка

        public
            UpgradeRadioGroup(string Caption,
                string[] elemText) //Конструктор принемает текст надписи и текста вариантов ответа
        {
            RadioPanel = new UpgradePanel(); //Создаём экземпляр панели
            Controls.Add(RadioPanel); //Привязываем её данному классу
            RadioLabel = new UpgradeLabel(); //Создаём надпись
            Controls.Add(RadioLabel); //Привязываем её данному классу
            RadioLabel.BringToFront(); //Выводим её на передний план
            RadioLabel.TextAlign = ContentAlignment.MiddleCenter; //Указываем выравнивание текста
            RadioLabel.Text = Caption; //Устанавливаем надпись
            RadioLabel.BorderStyle = BorderStyle.FixedSingle; //Устанавливаем рамку
            foreach (var RButton_Text in elemText) //Проходимся по всем вариантам ответа
            {
                var RB = new RadioButton(); //Для каждого создаём элемент для выбора
                RadioPanel.Controls.Add(RB); //Привязываем его созданной панели
                RB.Text = RButton_Text; //Устанавливаем соответсвующий текст
                Elements.Add(RB); //Добовляем в список элментов выбора
            }

            RepairElements(); //Перестраиваем внутренние элменты
        }

        public int ActiveIndex //Свойство активного индекса
        {
            get //Возвращает индекс выборанного элмента
            {
                for (var i = 0; i < Elements.Count; i++)
                    if (Elements[i].Checked)
                        return i;

                return -1;
            }
            set //либо может установить его
            {
                if (value >= 0 && value < Elements.Count) //Проверяем существуем ли такой элемнт
                    Elements[value].Checked = true;
            }
        }

        public string UniText //Св-во текста надписи
        {
            get => RadioLabel.Text;
            set
            {
                RadioLabel.Text = value;
                RepairElements();
            }
        }

        public Font UniFont //Св-во шрифта надписи
        {
            get => RadioLabel.Font;
            set
            {
                RadioLabel.Font = value;
                RepairElements();
            }
        }

        public void Move(int new_X, int new_Y)
        {
            Left = new_X;
            Top = new_Y;
            RepairElements();
        }

        public void Resize(int new_Width, int new_Height)
        {
            Width = new_Width;
            Height = new_Height;
            RepairElements();
        }

        public void SetGeometry(int new_X, int new_Y, int new_Width, int new_Height)
        {
            Left = new_X;
            Top = new_Y;
            Width = new_Width;
            Height = new_Height;
            RepairElements();
        }

        public void RepairElements() //перестройка внутренних элементов
        {
            //Просто востановление расположния
            RadioPanel.SetGeometry(2, 10, Width - 4, Height - 20);
            RadioPanel.BorderStyle = BorderStyle.FixedSingle;
            var sizeOfRadioLabelText = TextRenderer.MeasureText(RadioLabel.Text, RadioLabel.Font);
            RadioLabel.SetGeometry(10, 0,
                Math.Min(sizeOfRadioLabelText.Width, Width - 20) + 8, Math.Min(sizeOfRadioLabelText.Height, 30));
            for (var i = 0; i < Elements.Count; i++)
            {
                Elements[i].Font = RadioLabel.Font;
                if (i % 2 == 0)
                {
                    Elements[i].Size = new Size((RadioPanel.Width - 14) / 2, (RadioPanel.Height - 36) / (i / 2 + 1));
                    Elements[i].Location = new Point(5, 25 + (Elements[i].Height + 6) * (i / 2));
                }
                else
                {
                    Elements[i].Size = new Size((RadioPanel.Width - 14) / 2, (RadioPanel.Height - 36) / (i / 2 + 1));
                    Elements[i].Location = new Point(Elements[i].Width + 14, 25 + (Elements[i].Height + 6) * (i / 2));
                }
            }

            if (ActiveIndex == -1) ActiveIndex = 0; //Если ни один вариант не выбран то выбрато 0-й элмент
        }

        public void AddNewElement(string text) //Функция добавления нового варианты для выбора
        {
            var RB = new RadioButton();
            Elements.Add(RB);
            RB.Text = text;
            RadioPanel.Controls.Add(RB);
            RepairElements();
        }
    }
}