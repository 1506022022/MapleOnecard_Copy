public enum ESymbol
{
    Blue,Green,Red,Yellow, Violet
}
public enum ENumber
{
    one,two,three,four,five,six,attack_two,attack_three,change,king,jump,reverse,special
}
public class Card : ICard
{
    public ESymbol symbol { get; private set; }
    public ENumber number { get; private set; }

    public Card(ESymbol symbol, ENumber number)
    {
        this.symbol = symbol;
        this.number = number;
    }
}
