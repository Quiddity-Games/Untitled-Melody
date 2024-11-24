EXTERNAL setVariable(varName, varState)
EXTERNAL getVariable(varName)

VAR player_said_they_were_tired = false
VAR player_still_into_vocaloid = false
VAR player_opened_up_about_directing = false

#Conversation: Emerald
-> Texting_Sequence_1

=== Texting_Sequence_1 ===
Hey Emerald! I hope you’re doing well [emoji:smile] #Speaker: Amika
Sorry, I know it’s really late lol #Speaker: Amika
Do you remember those Vocaloid lyric videos we made back in high school? #Speaker: Amika
I just found a sketch of Memi in one of my notebooks #Speaker: Amika
Amika!!! Hey, it’s nice to hear from you! #Speaker: Emerald
I could never forget our little sun-shaped guy! #Speaker: Emerald
He’s still the mascot of my Vocaloid acct on YouTube #Speaker: Emerald
The one where we uploaded our song “Memories”! #Speaker: Emerald

* I worked SO hard on that #Speaker: Amika
I was up every night animating the video. I barely slept that month #Speaker: Amika
It shows! #Speaker: Emerald
That I worked super-hard, or that I barely slept? [emoji:sweat] #Speaker: Amika
The first one lmao!! #Speaker: Emerald

* It was *your* great song! #Speaker: Amika
I wish I could say I had your epic composition skills [emoji:smile] #Speaker: Amika
Hey you were up every night after school animating the lyric video for a MONTH. That means it’s your song too! #Speaker: Emerald


- You were a great artist even back in high school. No wonder you got that awesome job in Burbank right out of college [emoji:sunglasses] #Speaker: Emerald

* Aww man thanks!! #Speaker: Amika
You totally deserve it! #Speaker: Emerald

* It was hard, but I did it #Speaker: Amika
The interview process was a little intense, but I pulled through in the end [emoji:smile] #Speaker: Amika
I knew you would!! #Speaker: Emerald

* Sometimes you get lucky! #Speaker: Amika
Stop, don’t sell yourself short! #Speaker: Emerald
Seriously, don’t make me come all the way over there *shakes fist* #Speaker: Emerald
Ok ok, I’m super great and I deserve this extremely cool job, please don’t hurt me haha #Speaker: Amika
That’s better [emoji:rofl] #Speaker: Emerald

- It’s really nice of you to text. I had no idea you were still thinking about all those old lyric videos we made #Speaker: Emerald
Memi was adorable. I couldn’t have asked you to draw me a better mascot [emoji:hearteyes] #Speaker: Emerald
Tbh I actually haven’t made a new Vocaloid song in forever lol #Speaker: Emerald
How come? You always loved playing around with Miku, GUMI, and the others #Speaker: Amika
I guess I haven’t really used voice synthesizers for anything since I started music school #Speaker: Emerald




I saw you’ve kept up your Vocaloid blog, though! #Speaker: Amika
I did! I was just writing a post now actually [emoji:smile] #Speaker: Emerald
There’s this track by nostraightanswer that I LOVE. The lyrics really get me #Speaker: Emerald
- But anyways! Hbu? are you still listening to Crusher, Deco*27 and everybody? #Speaker: Emerald

*   I’m less into Vocaloid now #Speaker: Amika
        ~ setVariable("player_still_into_vocaloid", false)
        I just don’t have time for that kind of stuff anymore #Speaker: Amika
        Totally, I'm super busy these days too! But always glad to keep you in the loop lol #Speaker: Emerald
        I appreciate it :) I'm pretty far out of the loop at this point though. Like deep space #Speaker: Amika
        I'll rope you back in! Like how they space lassoed matt damon in that movie #Speaker: Emerald
        Haha good to know you'll always catch me up #Speaker: Amika

*   I still love Vocaloid! #Speaker: Amika
        ~ setVariable("player_still_into_vocaloid", true)
        I wish I could keep up with it more. I wouldn’t know any new songs unless you posted them haha #Speaker: Amika
        Yeah I bet you’re busy living it up in Cali! #Speaker: Emerald
        Hardly lol more like busy trying to stay afloat #Speaker: Amika


- I'm really sorry I haven’t messaged more often. #Speaker: Amika
No worries haha #Speaker: Emerald
How’s your job?? #Speaker: Emerald
I bumped into your mom when I was visiting home and she says you’re really busy #Speaker: Emerald
And how’s California so far? Don't tell me you say "stoked" and "beggle" now :O #Speaker: Emerald

*   It's great! And I don't lol #Speaker: Amika
        So great you’ll never come back haha #Speaker: Emerald
        Sorry, I wanted to make it over the summer! #Speaker: Amika
	Everyone’s putting in extra time to push out the new season of Blue Star: Alcyone. I couldn’t get free [emoji:anguish] #Speaker: Amika
        I’m just giving you a hard time! Maybe we can meet up over Thanksgiving? #Speaker: Emerald
        My folks are going to visit me out here actually… They want to escape the Illinois cold for a bit #Speaker: Amika
        Oh that sounds fun. But I’m coming after you for that raincheck! #Speaker: Emerald
        Absolutely! #Speaker: Amika

*   It’s been hard honestly #Speaker: Amika
        Everything's crazy. I haven’t been able to think straight or take a breath #Speaker: Amika
        Aw man! Hope you’re doing okay #Speaker: Emerald
        I’m alright, it’s just that working at an animation studio takes up so much mental space #Speaker: Amika
I feel like I’m playing catch up with everyone else and just trying to make it till the next deadline #Speaker: Amika
        I have no idea why they picked me. I might not be cut out for this #Speaker: Amika
        I felt the same about school you know #Speaker: Emerald
        Wait high school or college? #Speaker: Amika
        Both! #Speaker: Emerald
        Are you kidding? Or did I dream up all those amazing Chicago Youth Symphony recitals? #Speaker: Amika
Speaking of throwbacks! That feels like forever ago [emoji:joy] #Speaker: Emerald
I know it’s crazy haha #Speaker: Amika
	


*   Decent overall #Speaker: Amika
        I’m still getting in the groove #Speaker: Amika
        Getting in those California vibes [emoji:joy] #Speaker: Emerald
        Oh my gosh no I have no chill lately #Speaker: Amika
        Um when did you have chill to begin with?? #Speaker: Emerald
        Wow okay! Drag me then #Speaker: Amika


- How about you? How are things back in Illinois? #Speaker: Amika
Did you do anything fun during break? #Speaker: Amika
Break? #Speaker: Emerald
Your music program had fall break already right? #Speaker: Amika
Oh yeah I didn’t do anything though lol #Speaker: Emerald
Really? #Speaker: Amika

*   Well rest is important! #Speaker: Amika
        Lol as if! Sleep is for the weak #Speaker: Emerald

*   Don’t get TOO lazy lol #Speaker: Amika
        There are no breaks after graduation, you know #Speaker: Amika
        Thanks mom lol I just didn't feel like doing anything special #Speaker: Emerald

*   Busy with school then? #Speaker: Amika
        I guess. I'm keeping myself occupied #Speaker: Emerald

- What else are you doing this semester? #Speaker: Amika
Not much #Speaker: Emerald
I mean not much compared to you!! #Speaker: Emerald
Drawing all day and spotting celebrities, I’m jealous haha #Speaker: Emerald
No no it’s not like that! I’m just a prop artist, I don’t get to meet the voice talent. And all the spottings are over in silver lake xD #Speaker: Amika
You're closer to your dream than a lot of people get! You’re totally on your way to being a great director #Speaker: Emerald
Directing might not be my dream anymore actually #Speaker: Amika
Oh! Did something happen? #Speaker: Emerald

*   No, just being realistic #Speaker: Amika
        ~ setVariable("player_opened_up_about_directing", true)
        It's harder to be disappointed if you reset your expectations #Speaker: Amika
        Hmm. Does giving up actually save you the disappointment or just front load it? #Speaker: Emerald
        What do you mean? #Speaker: Amika
        Like are you actually changing your mind or are you letting your fear make the decision? #Speaker: Emerald
        If you don’t want to be a director anymore, that's totally your choice! Plans change #Speaker: Emerald
But I hope you’re not counting yourself out because you think you CAN’T do it. #Speaker: Emerald
You’re an amazing artist, Amika! You have so much to be proud of already. If it means anything, I’M proud of you :) #Speaker: Emerald
        Oh wow that means the world to me!! I appreciate you saying that Emerald, you always know how to cheer me up :) #Speaker: Amika

*   I’m not good enough #Speaker: Amika
        ~ setVariable("player_opened_up_about_directing", true)
        I'm nothing like the directors at my studio #Speaker: Amika
        What do you mean? #Speaker: Emerald
        I know I'm only starting out but I just feel invisible sometimes. I don't know why anyone would care what I have to say #Speaker: Amika
        Isn't it worth sharing your stories even if it feels like no one's listening? #Speaker: Emerald
        If you don’t want to be a director anymore, that's totally your choice! Plans change. #Speaker: Emerald
But I hope you’re not counting yourself out because you think you CAN’T do it. #Speaker: Emerald
You’re an amazing artist, Amika! You have so much to be proud of already. If it means anything, I’M proud of you :) #Speaker: Emerald
        Oh wow that means the world to me!! I appreciate you saying that Emerald, you always know how to cheer me up :) #Speaker: Amika

*   Nvm! Don't worry about it #Speaker: Amika
        ~ setVariable("player_opened_up_about_directing", false)
        I'm fine, everything's good #Speaker: Amika

- Sorry for bringing things down #Speaker: Amika
You have nothing to be sorry for! #Speaker: Emerald
Well if I ever do direct a film you’re my first choice for music! I know you would write a killer score #Speaker: Amika
Have you been working on any new songs for class? #Speaker: Amika
Not really - I’ve actually been spending more time listening lately. #Speaker: Emerald
Like I was saying, I’m working on a blog post right now about nostraightanswer. Have you heard of them? #Speaker: Emerald

* I haven’t! #Speaker: Amika
Ooh, you might like nostraightanswer! Their sound is so awesome #Speaker: Emerald

* That rings a bell… #Speaker: Amika
nostraightanswer is this awesome Vocaloid artist! #Speaker: Emerald

- I've had their song “wishing well” on repeat. I find it soothing, it’s been keeping me going this year. #Speaker: Emerald
Soothing? How so? #Speaker: Amika
If you listen to it you'll see! If you need a pressure valve from the rush of the city it might help #Speaker: Emerald
Plus, it has beautiful lyrics. I think about them all the time #Speaker: Emerald
Anyway sorry to head out so soon but I have to finish up this post and head off to work, I got stuck with the graveyard again rip #Speaker: Emerald
Your school library has a graveyard shift?? #Speaker: Amika
Oh I work off campus now! So yeah crazy hours #Speaker: Emerald
Bummer… Maybe they’re hiring somewhere else on campus? #Speaker: Amika
I checked but all the on campus jobs are taken actually #Speaker: Emerald
All of them? That sucks. #Speaker: Amika
Yeah but I really do have to go now, you should look up the song! #Speaker: Emerald

*   Sure, I'd love to! :D #Speaker: Amika
        ~ setVariable("player_said_they_were_tired", false)
        I'll give it a listen tonight #Speaker: Amika
        Oh awesome!! Lmk what you think! #Speaker: Emerald


*   If I’m not too tired  #Speaker: Amika
        ~ setVariable("player_said_they_were_tired", true)
        It’s been a pretty busy day [emoji:anguish] #Speaker: Amika
	No worries, I understand! #Speaker: Emerald



- Anyway, talk later? #Speaker: Emerald
Definitely! I should probably get some sleep #Speaker: Amika
Maybe I’ll listen to nostraightanswer’s song while I crawl into bed #Speaker: Amika
A soothing Vocaloid lullaby [emoji:smile] #Speaker: Emerald
I guess so, lol! #Speaker: Amika
Sweet dreams, and let me know if you find any more sketches of Memi! [emoji:hearteyes] #Speaker: Emerald


#end
->END




