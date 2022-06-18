
public class mStats
{

    public int HP;
    public int Armor;
    public int Str;
    public int Avoidance;
    public float Speed;
    public float AtkSpeed;
    public int Shots;
    public int SpecialShot0;
    public int SpecialShot1;
    public int SpecialShot2;

    public mStats()
    {
        HP = Armor = Str = Avoidance = Shots = SpecialShot0 = SpecialShot1 = SpecialShot2 = 0; Speed = AtkSpeed = 0.0f;
    }

    public void load(int hp, int armor, int str, int avoidance, float speed, float atkspeed, int shots, int ss0, int ss1, int ss2)
    {
        HP = hp; Armor = armor;  Str = str; Avoidance = avoidance; Speed = speed; AtkSpeed = atkspeed; Shots = shots; SpecialShot0 = ss0; SpecialShot1 = ss1; SpecialShot2 = ss2;
    }


}
