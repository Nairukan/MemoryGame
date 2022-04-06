using System;
using System.Collections.Generic;
using System.Drawing;
using MermoryGame.Control;

namespace MermoryGame
{
    public class GameHubForm : AbsForm //Класс окно игровой комнаты(лобби), наследуемся от базового класа
    {
        public List<string> ActiveUsername = new List<string>(); //Список логинов авторизированных игроков
        public List<UpgradeButton> PlayersButton = new List<UpgradeButton>(); //Список кнопок в меню лобби

        public GameHubForm() //После выполнения конструктора род. класса выполняем следующее
        {
            Text = "Игра домино. Комната ожидания игроков"; //Устанавливаем текст в названии окна
            countFontLayers++; //Увеличиваем кол-во множеств элментов с общими шрифтами
            var LobbyText = AddLabel("LobbyText"); //Добавляем надпись на форму
            LobbyText.SetGeometry(40, 15, 340, 75); //Устанавливаем её размеры и позицию
            LobbyText.TextAlign = ContentAlignment.MiddleCenter; //Устанавливаем выравнивание текста
            LobbyText.Text = "Игроки за столом:"; //Устанавливаем текст
            AddToFontLevel(1, LobbyText); //Добавляем эту надпись к 1-ому слою одношрифтовых элментов

            var PlayersPanel = AddPanel("PlayersPanel"); //Создаём панель
            PlayersPanel.SetGeometry(45, 90, 340, 265); //Меняем позицию и размер
            PlayersPanel.BackColor = Color.LightGray; //Устанавливаем цвет фона элемента
            countFontLayers++; //Увеличиваем кол-во множеств элментов с общими шрифтами


            var AddPlayerButton = AddButton("AddPlayerButton", PlayersPanel); //Создаём кнопку добавления игрока
            AddPlayerButton.Text = "+Добавить участника"; //Устанавливаем текст
            AddPlayerButton.SetGeometry(10, 5, 320, 60);
            AddToFontLevel(2, AddPlayerButton);
            AddPlayerButton.Click += AddPlayerButtonClick; //Привязываем событию нажатия функцию
            //Дальше точно так же по аналогии

            var Player1Button = AddButton("Player1Button", PlayersPanel);
            Player1Button.SetGeometry(10, 5, 320, 60);
            AddToFontLevel(2, Player1Button);
            Player1Button.Hide();
            PlayersButton.Add(Player1Button);
            Player1Button.Click += EraseSomePlayers;

            var Player2Button = AddButton("Player2Button", PlayersPanel);
            Player2Button.SetGeometry(10, 70, 320, 60);
            AddToFontLevel(2, Player2Button);
            Player2Button.Hide();
            PlayersButton.Add(Player2Button);
            Player2Button.Click += EraseSomePlayers;

            var Player3Button = AddButton("Player3Button", PlayersPanel);
            Player3Button.SetGeometry(10, 135, 320, 60);
            AddToFontLevel(2, Player3Button);
            Player3Button.Hide();
            PlayersButton.Add(Player3Button);
            Player3Button.Click += EraseSomePlayers;

            var Player4Button = AddButton("Player4Button", PlayersPanel);
            Player4Button.SetGeometry(10, 200, 320, 60);
            AddToFontLevel(2, Player4Button);
            Player4Button.Hide();
            PlayersButton.Add(Player4Button);
            Player4Button.Click += EraseSomePlayers;

            var SettingsGameText = AddLabel("SettingsGameText");
            SettingsGameText.SetGeometry(390, 15, 340, 75);
            SettingsGameText.TextAlign = ContentAlignment.MiddleCenter;
            SettingsGameText.Text = "Настройки игры:";
            AddToFontLevel(1, SettingsGameText);

            var SettingsPanel = AddPanel("SettingsPanel");
            SettingsPanel.SetGeometry(390, 90, 340, 265); //Меняем позицию и размер
            SettingsPanel.BackColor = Color.LightGray;
            countFontLayers++;

            var ChildModeRadioGroup =
                AddRadioGroup("ChildModeRadioGroup",
                    SettingsPanel); //Создаём элмент выбора одного из множества(Почитай ниже про него)
            ChildModeRadioGroup.RadioLabel.Text = "Детский режим"; //Надпись на элементе
            ChildModeRadioGroup.SetGeometry(10, 160, 320, 100); //Размер и позиция
            ChildModeRadioGroup.AddNewElement("Нет"); //Первый выриант для выбора
            ChildModeRadioGroup.AddNewElement("Да"); //Второй вариант
            AddToFontLevel(3, SettingsPanel);

            var StartGameButton = AddButton("StartGameButton"); //Создаём элмент управления
            StartGameButton.Text = "Начать игру"; //Меняем содержимое
            StartGameButton.SetGeometry(80, 370, 480, 70); //Меняем позицию и размер
            StartGameButton.Click += StartGameButtonClick;
            //По аналогии дальше

            SetMaxFont(); //Вызываем функцию приведения к красивому оформлению сожержимого интерфейса
        }

        private void StartGameButtonClick(object sender, EventArgs args) //Нажатие на кнопку "Начать игру"
        {
            NotifyObservers("StartGame"); //Уведомляем всех наблюдателей попытке начала игры
        }

        private void EraseSomePlayers(object sender, EventArgs args) //Нажатие на кнопку пользователя(Удаления из лобби)
        {
            NotifyObservers("LogoutPlayer " + ((UpgradeButton) sender).Name.Substring(6, 1));
        }

        private void AddPlayerButtonClick(object sender, EventArgs args) //Кнопка добавления нового игрока
        {
            NotifyObservers("AddPlayer");
        }

        public void NotifyObservers(string message) //Функция уведомления наблюдателей
        {
            foreach (var o in observers) //Проходит по всем элементам следящим за ним 
                o.Update(message.Split()); //Обновляя их с соответсвующим сообщением
            SetMaxFont();
        }
    }
}