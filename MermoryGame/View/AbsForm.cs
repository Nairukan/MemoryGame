using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MermoryGame.Control;

namespace MermoryGame
{
    public abstract partial class AbsForm : Form //Определяем базовый класс для всех окон который будут в проекте,
                                                 //наследуемся от стандартной формы
    {
        //Создаём список списков элементов интерфейса, который будет отображать распределение эелементов интерфейса по
        //слоям размеров шрифтов
        private List<List<System.Windows.Forms.Control> > FontLevels=new List<List<System.Windows.Forms.Control>>();

        //Св-во Кол-во Слоёв Шрифта
        public int countFontLayers
        {
            get { return FontLevels.Count;} //При запросе значенения возвращаем размер
            set //При запросе Установки значения
            {
                value = Math.Max(value, 1); //Гарантируем что у нас останется хотя бы один слой(базовый)
                if (value < FontLevels.Count)  //Если установленное число меньше текущего размера, то
                {
                    for (int i = value; i < FontLevels.Count; i++) //Проходим по "слоям шрифта" которые необходимо удалить
                    {
                        foreach (var elemInterface in FontLevels[i]) //Проходим по всем элементам на этих "слоях"
                        {
                            FontLevels[value - 1].Add(elemInterface); //Переносим их на последний "слой" котоый останется
                        }
                        FontLevels[i].Clear(); //Очищаем удаляемый слой от элементов
                    }
                    FontLevels.RemoveRange(value, FontLevels.Count-value); //Удаляем необходимые уровни
                }else  //Иначе(если новый размер больше текущего)
                    for (int i=0; i<value-FontLevels.Count; i++) //Тогда необходимое кол-во раз 
                        FontLevels.Add(new List<System.Windows.Forms.Control>()); //Добавляем новый "слой шрифта"
            }
        }

        public void AddToFontLevel(int ind, System.Windows.Forms.Control Element) //Функция Добавления элемента к
                                                                                  //некоторому "слою"
        {
            if (ind >= 0 && ind < countFontLayers) //Проверяем на валидность полученного значения
            {
                FontLevels[ind].Add(Element);
            }
            else //Если вышло за границы
                throw new System.Exception("Out of interval(FontLevels)!"); //Генерируем исключение в котором
                                                                            //информируем от выходе за пределы
        }
    
        public AbsForm() //Конструктор класса
        {
            
            FontLevels.Add(new List<System.Windows.Forms.Control>()); //Создаём базовый(0-й) уровень шрифтов
            InitializeComponent(); //Инициализируем компоненты установленный в Designer для данного окна
            FontLevels[0].AddRange(this.Controls.Cast<System.Windows.Forms.Control>()); //К базовому уровню
                                                                            //добавляем все элементы интерфейса окна
        }
        
        
        
        public void SetMaxFont() //Функция подгонки размера шрифтов
        {
            foreach (var level in FontLevels) //Проходим по всем "слоям шрифтов"
            {
                //Пытаемся установить максимально возможный размер текста для всех элментов уровня, чтобы содержимое
                //корректно отображалось(вмещалось в границы элемента)
                
                int maxFont = 70;//Устанавливаем максимально возможный размер шрифта изначально 70
                foreach (System.Windows.Forms.Control SomeElem in level) //Проходим по всем элменам интерфейса на данном уровне
                {
                    Font font = new Font(SomeElem.Font.Name, maxFont, SomeElem.Font.Style); //Копируем шрифт из данного
                                                                                            //элемента но с заданным размерои
                    Size temp = TextRenderer.MeasureText(SomeElem.Text, font);//Получаем размер Содержимого для данного шрифта
                    while (SomeElem.Height < temp.Height || SomeElem.Width < temp.Width) //Пока при данном шрифте размер содержимого больше границ
                    {
                        maxFont--; //Уменьшаем значение максимально возможного размера шрифта
                        font = new Font(SomeElem.Font.Name, maxFont, SomeElem.Font.Style); //обновляем шрифт новым размером
                        temp = TextRenderer.MeasureText(SomeElem.Text, font); //Получаем размер Содержимого для нового шрифта
                    }
                }

                foreach (System.Windows.Forms.Control SomeElem in level) //Проходим по всем элменам интерфейса на данном уровне
                {
                    SomeElem.Font = new Font(SomeElem.Font.Name, maxFont, SomeElem.Font.Style); //Устанавлиываем такой
                                                                                                //же шрифт но с другим размером
                }
            }
        }

        bool tryGetControlWithName(string Name, out System.Windows.Forms.Control retrunControl) //Функция попытки
                                                                                                //получения элемента интерфейса по его имени
        {
            retrunControl = null; //заглушка для варианты который вернёт false
            System.Windows.Forms.Control[] answer = this.Controls.Find(Name, false); //Массив всех элментов
                                                                                     //интерфейса с заданным именем
            if (answer.Length == 0) return false; //Если не было найдено  возвращаем состояние ошибкаи
            retrunControl = answer[0]; //иначе сохраняем значений
            return true; //Возвращаем состояение успеха
        }

        
        public UpgradeButton AddButton(String name) //Функция добавления  к окну улучшенной кнопки
        {
            UpgradeButton button = new UpgradeButton(); //Создаём новую улучшенную кнопку
            button.Name = name; //Присваиваем ему установленное имя
            this.Controls.Add(button); //Добовляем его к  окну
            FontLevels[0].Add(button); //К базовому уровню добавляем элемент интерфейса окна
            return button; //Возвращаем значение
        }
        
        public UpgradeLineEdit AddLineEdit(String name) //Функция добавления  к окну улучшенного поля для ввода
        {
            //по аналогии
            UpgradeLineEdit lineEdit = new UpgradeLineEdit();
            lineEdit.Name = name;
            this.Controls.Add(lineEdit);
            FontLevels[0].Add(lineEdit); 
            return lineEdit;
        }
        
        public UpgradeLabel AddLabel(String name) //Функция добавления  к окну улучшенной кнопки
        {
            //По аналогии
            UpgradeLabel label = new UpgradeLabel();
            label.Name = name;
            this.Controls.Add(label);
            FontLevels[0].Add(label);
            return label;
        }

    }
}
