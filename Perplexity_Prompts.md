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

