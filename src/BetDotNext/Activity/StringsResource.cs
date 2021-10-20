namespace BetDotNext.Activity
{
  public static class StringsResource
  {
    public static string StartActivityMessage = "Здравия желаю!\r\n" + 
                                                "Я бот, который принимает ставки на рейтинг спикеров DotNext.\r\n" +
                                                "Спикеры: https://dotnext-piter.ru/2021/spb/people/ \r\n" +
                                                "Результаты ставок: http://bookmakerboard.azurewebsites.net/ \r\n" +
                                                "Чат для обсуждения и вопросов: https://t.me/dotnext_rates" +
                                                "\r\n" +
                                                "У Вас есть 1000 баллов, которые Вы можете ставить на то, что спикер попадет в номинации рейтинга: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, top3, top5, top10\r\n" +
                                                "\r\n" +
                                                "После публикации организаторами рейтинга спикеров на хабре, если Ваша ставка сработала, то:\r\n" +
                                                "количество баллов в номинации top10 - удваивается\r\n" +
                                                "количество баллов в номинации top5 - утраивается\r\n" +
                                                "количество баллов в номинации top3 - учетверяется\r\n" +
                                                "количество баллов поставленных на определенные места (1-10) - упятеряется\r\n" +
                                                "\r\n" +
                                                "при равном колличестве итоговых баллов, преимущество получают ставки сделанные раньше по времени \r\n" +
                                                "Справка по командам: /help \r\n";

    public static readonly string AcceptedBetActivityMessage = "Формат принятия ставки следующий: \r\n" +
                                                               "[спикер] - [ставка] - [номинация] \r\n" +
                                                               "Например:\r\n" +
                                                               "Christophe Nasarre - 300 - 1\r\n" +
                                                               "Jerome Laban - 100 - top3\r\n" +
                                                               "Lukasz Pyrzyk - 10 - top10\r\n" +
                                                               "Алексей Мерсон - 500 - top5\r\n" +
                                                               "\r\n" +
                                                               "значения ставок - целочисленные, повторная ставка на спикера в той же номинации считается корректировкой предыдущей ставки";

    public static readonly string LoadingMessage = "Обрабатываю Вашу команду...";

    public static readonly string BetActivityUnexpectedFormatMessage = "Ошибка - неверный формат команды";

    public static readonly string SuccessBetActivity = "Команда успешно обработана.\r\n" + 
                                                       "Осталось баллов: {0}";

    public static readonly string FailCreatedActivityMessage = "Не удалось создать ставку.\r\n" + 
                                                               "Попробуйте выполнить команду снова";

    public static readonly string FailDeleteActivityMessage = "Не удалось удалить ставку.\r\n" + 
                                                              "Попробуйте выполнить команду снова";

    public static readonly string RemoveBetActivityMessage = "Формат снятия ставки слядующий:\r\n" + 
                                                             "[спикер] - 0 - [номинация]\r\n" +
                                                             "[спикер] - 0   (снять все ставки на спикера)";

    public static readonly string BetRateMustZero = "Ошибка - cтавка должна быть равна 0";

    public static readonly string BetRateIsNotZerro = "Ошибка - ставка не должна быть равна 0";

    public static readonly string BetRateIsNotEnough = "Ошибка - для данной ставки у Вас недостаточно баллов";

    public static readonly string SuccessfullyBetRemove = "Ваша ставка успешно удалена";

    public static readonly string SuccessfullyBetsRemove = "Ваши ставки успешно удалены";


    public static readonly string NotExistBidder = "Ошибка - сначала вы должны сделать ставку";

    public static readonly string CurrentScoreRemoveMessage = "Все ваши ставки успешно удалены.\r\n" +
                                                              "Осталось баллов: {0}";

    public static readonly string SpeakerNotFound = "Ошибка - указанный спикер отсутствует в перечене спикеров: https://dotnext-piter.ru/2021/spb/people/";

    public static readonly string HelpText = "Бот понимает команды:\r\n" +
                                             "\r\n" +
                                             "/start - начало работы с ботом\r\n" +
                                             "/bet - сделать ставку на спикера (указывать перед каждой ставкой)\r\n" +
                                             "/removebet - снять поставленную ранее ставку\r\n" +
                                             "/removeall - снять все ранее поставленные ставки\r\n" +
                                             "/score - отобразить свой текущий счет\r\n" +
                                             "/help - выдать эту справку";

    public static readonly string IncorectNomination = "Ошибка - некорректный формат номинации, возможные варианты: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, top3, top5, top10";

    public static readonly string NominationNotEmpty = "Ошибка - номинация не может быть пустой";

    public static readonly string GettingCurrentScoreException = "Не удалось получить текущее состояние счёта";

    public static readonly string CurrentScoreMessage = "Осталось баллов: {0}";
  }
}
