# Perplexity Prompts

## Prompt 1
public Time GetFullTimeUntil(DateTime expiration)
{

}

Make this function, you can use this helper:
public int GetSecondsUntil(DateTime expiration)
{
        TimeSpan diff = expiration - DateTime.UtcNow;
        return (int)diff.TotalSeconds;
}

the entity has 4 int properties: Days, Hours, Minutes, Seconds

## Promp 2
<div class="row justify-content-around align-items-center my-3 statBlock">
        <p class="col-5 text-center text-lg-start fs-3 text-nowrap m-0 fw-bold">Winner:</p>
	<p class="col-5 text-center text-lg-end fs-4 text-nowrap my-0">@Model.WinnerName</p>
	<p class="col-5 text-center text-lg-start fs-5 text-nowrap m-0 fw-bold">(a.k.a.:)</p>
	<p class="col-5 text-center text-lg-end fs-4 text-nowrap my-0">@Model.WinnerOtherName</p>
</div>

On big screen it should stay like this, on small screen the 2nd and 3th p have to be swapped


## Prompt 3
all even nth of type css

