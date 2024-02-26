public static class RuleSystem
{
    public static bool Rule(Card info)
    {

        // 선택된 카드의 정보를 불러온다.
        var card = Focus.Singleton.info;
        if (card == null) return false;

        // 제출 가능한 카드인지 확인한다.
        if (info.number == ENumber.special && info.symbol == ESymbol.Violet) return true;
        if (AttackFlame.Singleton.Count > 0)
        {

            if (info.number == ENumber.special && info.symbol == ESymbol.Red) return true;
            if (info.number == ENumber.special && info.symbol == ESymbol.Yellow) return true;

            if (info.number >= card.number)
            {
                if (info.symbol == card.symbol)
                {
                    if (info.number == ENumber.attack_two) return true;
                    if (info.number == ENumber.attack_three) return true;
                }
                else
                {
                    if (info.number == ENumber.special) return false;
                    if (info.number == card.number) return true;
                }
            }
        }
        else
        {

            if (card.number == info.number) return true;
            if (card.symbol == info.symbol) return true;
        }

        return false;
    }
}
