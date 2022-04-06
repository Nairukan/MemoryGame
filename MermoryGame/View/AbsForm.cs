using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MermoryGame.Control;
using MermoryGame.View.UpgradeControls;

namespace MermoryGame
{
    public abstract partial class
        AbsForm : Form, IObservable //Определяем базовый класс для всех окон который будут в проекте,
        //наследуемся от стандартной формы, содержит интерфейс наблюдаемого объекта
    {
        //Создаём список списков элементов интерфейса, который будет отображать распределение эелементов интерфейса по
        //слоям размеров шрифтов
        private readonly List<List<System.Windows.Forms.Control>> FontLevels =
            new List<List<System.Windows.Forms.Control>>();


        public List<IObserver> observers; //Список наблюдателей 

        public AbsForm() //Конструктор класса
        {
            observers = new List<IObserver>();
            FontLevels.Add(new List<System.Windows.Forms.Control>()); //Создаём базовый(0-й) уровень шрифтов
            InitializeComponent(); //Инициализируем компоненты установленный в Designer для данного окна
            FontLevels[0].AddRange(Controls.Cast<System.Windows.Forms.Control>()); //К базовому уровню
            //добавляем все элементы интерфейса окна
        }

        //Св-во Кол-во Слоёв Шрифта
        public int countFontLayers
        {
            get => FontLevels.Count; //При запросе значенения возвращаем размер
            set //При запросе Установки значения
            {
                value = Math.Max(value, 1); //Гарантируем что у нас останется хотя бы один слой(базовый)
                if (value < FontLevels.Count) //Если установленное число меньше текущего размера, то
                {
                    for (var i = value;
                        i < FontLevels.Count;
                        i++) //Проходим по "слоям шрифта" которые необходимо удалить
                    {
                        foreach (var elemInterface in FontLevels[i]) //Проходим по всем элементам на этих "слоях"
                            FontLevels[value - 1]
                                .Add(elemInterface); //Переносим их на последний "слой" котоый останется
                        FontLevels[i].Clear(); //Очищаем удаляемый слой от элементов
                    }

                    FontLevels.RemoveRange(value, FontLevels.Count - value); //Удаляем необходимые уровни
                }
                else //Иначе(если новый размер больше текущего)
                {
                    for (var i = 0; i < value - FontLevels.Count; i++) //Тогда необходимое кол-во раз 
                        FontLevels.Add(new List<System.Windows.Forms.Control>()); //Добавляем новый "слой шрифта"
                }
            }
        }

        public void RegisterObserver(IObserver o) //Добавление наблюдателя
        {
            observers.Add(o);
        }

        public void RemoveObserver(IObserver o) //Удаление наблюдателя
        {
            observers.Remove(o);
        }

        public void NotifyObservers(string message)
        {
            foreach (var o in observers) o.Update(new object[] {"Update"});
        }

        public void AddToFontLevel(int ind, System.Windows.Forms.Control Element) //Функция Добавления элемента к
            //некоторому "слою"
        {
            if (ind >= 0 && ind < countFontLayers) //Проверяем на валидность полученного значения
            {
                foreach (var FontControls in FontLevels)
                    if (FontControls.Contains(Element))
                        FontControls.Remove(Element);
                FontLevels[ind].Add(Element);
            }
            else //Если вышло за границы
            {
                throw new Exception("Out of interval(FontLevels)!"); //Генерируем исключение в котором
            }

            //информируем от выходе за пределы
        }


        public void SetMaxFont() //Функция подгонки размера шрифтов
        {
            foreach (var level in FontLevels) //Проходим по всем "слоям шрифтов"
            {
                //Пытаемся установить максимально возможный размер текста для всех элментов уровня, чтобы содержимое
                //корректно отображалось(вмещалось в границы элемента)

                var maxFont = 70; //Устанавливаем максимально возможный размер шрифта изначально 70
                foreach (var SomeElem in level) //Проходим по всем элменам интерфейса на данном уровне
                    if (SomeElem is UpgradeRadioGroup)
                    {
                        var font = new Font(((UpgradeRadioGroup) SomeElem).UniFont.Name, maxFont,
                            ((UpgradeRadioGroup) SomeElem).UniFont.Style); //Копируем шрифт из данного
                        //элемента но с заданным размерои
                        var tempSize = TextRenderer.MeasureText(((UpgradeRadioGroup) SomeElem).UniText,
                            font); //Получаем размер Содержимого для данного шрифта
                        while (30 < tempSize.Height || SomeElem.Width - 20 < tempSize.Width
                        ) //Пока при данном шрифте размер содержимого больше границ
                        {
                            maxFont--; //Уменьшаем значение максимально возможного размера шрифта
                            font = new Font(((UpgradeRadioGroup) SomeElem).UniFont.Name, maxFont,
                                ((UpgradeRadioGroup) SomeElem).UniFont.Style); //обновляем шрифт новым размером
                            tempSize = TextRenderer.MeasureText(((UpgradeRadioGroup) SomeElem).UniText,
                                font); //Получаем размер Содержимого для данного шрифта
                        }
                    }
                    else
                    {
                        var font = new Font(SomeElem.Font.Name, maxFont,
                            SomeElem.Font.Style); //Копируем шрифт из данного
                        //элемента но с заданным размерои
                        var tempSize = TextRenderer.MeasureText(SomeElem.Text,
                            font); //Получаем размер Содержимого для данного шрифта
                        while (SomeElem.Height < tempSize.Height || SomeElem.Width < tempSize.Width
                        ) //Пока при данном шрифте размер содержимого больше границ
                        {
                            maxFont--; //Уменьшаем значение максимально возможного размера шрифта
                            font = new Font(SomeElem.Font.Name, maxFont,
                                SomeElem.Font.Style); //обновляем шрифт новым размером
                            tempSize = TextRenderer.MeasureText(SomeElem.Text,
                                font); //Получаем размер Содержимого для нового шрифта
                        }
                    }

                foreach (var SomeElem in level) //Проходим по всем элменам интерфейса на данном уровне
                    if (SomeElem is UpgradeRadioGroup)
                        ((UpgradeRadioGroup) SomeElem).UniFont = new Font(((UpgradeRadioGroup) SomeElem).UniFont.Name,
                            maxFont,
                            ((UpgradeRadioGroup) SomeElem).UniFont
                            .Style); //Устанавлиываем такой же шрифт но с другим размером
                    else
                        SomeElem.Font =
                            new Font(SomeElem.Font.Name, maxFont, SomeElem.Font.Style); //Устанавлиываем такой
                //же шрифт но с другим размером
            }
        }

        public bool
            tryGetControlWithName(string Name, out System.Windows.Forms.Control retrunControl) //Функция попытки
            //получения элемента интерфейса по его имени
        {
            retrunControl = null; //заглушка для варианты который вернёт false
            foreach (var level in FontLevels)
            foreach (var control in level)
                if (control.Name == Name)
                {
                    retrunControl = control;
                    return true;
                }

            return false; //Возвращаем состояение успеха
        }


        public UpgradeButton
            AddButton(string name,
                System.Windows.Forms.Control Owner = null) //Функция добавления  к окну улучшенной кнопки
        {
            var button = new UpgradeButton(); //Создаём новую улучшенную кнопку
            button.Name = name; //Присваиваем ему установленное имя
            if (Owner == null)
                Controls.Add(button); //Добовляем его к  окну
            else
                Owner.Controls.Add(button);
            FontLevels[0].Add(button); //К базовому уровню добавляем элемент интерфейса окна
            return button; //Возвращаем значение
        }

        public UpgradeLineEdit
            AddLineEdit(string name,
                System.Windows.Forms.Control Owner = null) //Функция добавления  к окну улучшенного поля для ввода
        {
            //по аналогии
            var lineEdit = new UpgradeLineEdit();
            lineEdit.Name = name;
            if (Owner == null)
                Controls.Add(lineEdit); //Добовляем его к  окну
            else
                Owner.Controls.Add(lineEdit);
            FontLevels[0].Add(lineEdit);
            return lineEdit;
        }

        public UpgradeLabel
            AddLabel(string name,
                System.Windows.Forms.Control Owner = null) //Функция добавления  к окну улучшенной кнопки
        {
            //По аналогии
            var label = new UpgradeLabel();
            label.Name = name;
            if (Owner == null)
                Controls.Add(label); //Добовляем его к  окну
            else
                Owner.Controls.Add(label);
            FontLevels[0].Add(label);
            return label;
        }

        public UpgradePanel
            AddPanel(string name,
                System.Windows.Forms.Control Owner = null) //Функция добавления  к окну улучшенной кнопки
        {
            //По аналогии
            var panel = new UpgradePanel();
            panel.Name = name;
            if (Owner == null)
                Controls.Add(panel); //Добовляем его к  окну
            else
                Owner.Controls.Add(panel);
            FontLevels[0].Add(panel);
            return panel;
        }

        public UpgradeRadioGroup
            AddRadioGroup(string name,
                System.Windows.Forms.Control Owner = null) //Функция добавления  к окну улучшенной кнопки
        {
            //По аналогии
            var radioGroup = new UpgradeRadioGroup("", new string[] { });
            radioGroup.Name = name;
            if (Owner == null)
                Controls.Add(radioGroup); //Добовляем его к  окну
            else
                Owner.Controls.Add(radioGroup);
            FontLevels[0].Add(radioGroup);
            return radioGroup;
        }
    }
}