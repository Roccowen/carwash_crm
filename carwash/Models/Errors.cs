using System;
using System.Collections.Generic;
using System.Text;

namespace carwash.Models
{
    public static class Errors
    {
        public const string UncorrectPhoneFormat = "Номер должен начинаться с +7 или 8";
        public const string ConnectionProblem = "Проблемы с соединением";
        public const string WrongLoginOrPassword = "Неверный логин или пароль";
        public const string PhoneMustNotBeEmpty = "Номер не должен быть пустым";
        public const string PasswordMustNotBeEmpty = "Пароль не должен быть пустым";
        public const string NameMustNotBeEmpty = "Имя не должно быть пустым";
        public const string ThisPhoneAlreadyTaken = "Номер уже занят";
    }
}
