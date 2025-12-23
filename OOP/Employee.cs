class Employee
{
    //fields
    string name = string.Empty;
    int age;
    int price;
    short quantity;

    public int Price
    {
        get { return price; }
        set
        {
            if(value< 0) throw new Exception("Price must be >=0");
            price = value;
        }
    }

    public short Quantity
    {
        get { return quantity; }
        set
        {
            if (value <= 0)
            {
                quantity = 1;
            }
            else
            {
                quantity = value;
            }          
        }
    }

    public int Amount
    {
        get { return price * quantity; }
    }   
    public int Age
    {
        get { return age; }
        set { age = value; }
    }
    //property
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
}