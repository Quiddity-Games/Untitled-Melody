VAR player_said_they_were_busy = false
VAR player_still_into_vocaloid = true
VAR player_opened_up_about_directing = false
VAR player_said_song_gets_them = false

#Conversation: Emerald
-> Texting_Sequence_2

=== Texting_Sequence_2 ===
hey Emerald I listened to wishing well last night! #Speaker: Amika

{
    -player_said_they_were_busy:
        oh awesome! I’m so glad you made time for it since you said you were busy :) did you like it? #Speaker: Emerald
        
    -not player_still_into_vocaloid:
        yay! I’m so glad you gave it a chance even though you’re not involved with vocaloid anymore :) did you like it? #Speaker: Emerald
    
    -else:
        yay I knew you’d like it! #Speaker: Emerald
        wait you did like it right #Speaker: Emerald
}

*   for sure! #Speaker: Amika
        it was really cool to listen to #Speaker: Amika
        yeah it’s one of my favorites! I’m listening to it all the time lately #Speaker: Emerald
        
*   oh I LOVED it!! #Speaker: Amika
        it was honestly super-cool. I feel like it’s really my style of music #Speaker: Amika
        it was like, this song gets me, lol [emoji:joy] #Speaker: Amika
        yeah, me too!!! #Speaker: Emerald
        
*   it was OK #Speaker: Amika
        I think I need to get used to listening to vocaloid again #Speaker: Amika
        I’m not used to that kind of sound #Speaker: Amika
        ahh no worries! #Speaker: Emerald
        I hope it was at least interesting to listen to #Speaker: Emerald
        oh yeah it was for sure! #Speaker: Amika
        
- the song made me think a little, actually #Speaker: Amika
nice! what about? #Speaker: Emerald
there’s this one line that stuck with me . . . #Speaker: Amika

*   “I'll tell the lies, compromise with a wishing well” #Speaker: Amika
        oh! why did that line stand out to you? #Speaker: Emerald
        
        **  I feel like a liar #Speaker: Amika
                no one at work knows how freaked out I am about messing up #Speaker: Amika
                
                {
                    -player_opened_up_about_directing:
                        nooo I thought we cured your impostor syndrome already!! #Speaker: Emerald
                        I know I know it’s an ongoing battle lol #Speaker: Amika
                }
                
                aw Amika that’s a tough spot to be in #Speaker: Emerald
                but it’s not a lie to look like you have it together even if you really don’t! #Speaker: Emerald
                it’s totally a lie of omission! #Speaker: Amika
                
        **  I’ve had to compromise #Speaker: Amika
                California wasn’t what I expected #Speaker: Amika
                
                {
                    -player_opened_up_about_directing:                
                        that’s right, we talked about impostor syndrome. do I need to type a whole nother pep talk about what an AMAZING artist you are?? #Speaker: Emerald
                        well I wouldn’t turn it down haha #Speaker: Amika
                        but it’s not just work. sometimes it’s hard to feel like you’re making a genuine connection with anyone #Speaker: Amika
                }
                
                like everyone’s just trying to get ahead and it can feel like you’re being left behind #Speaker: Amika
                yeah I know what that’s like #Speaker: Emerald
                but at least you have your classmates to go through it with you. sometimes I feel totally alone out here #Speaker: Amika
                
*   “I might cry, but I'll love myself” #Speaker: Amika
        how does that line make you feel? #Speaker: Emerald

        **  it’s dark #Speaker: Amika
                it does sound like a vulnerable state to be in #Speaker: Emerald
                what would you say to the person in the song? or anyone who felt that way? #Speaker: Emerald
                well do you remember when I told you about kintsugi? #Speaker: Amika
                the gold cracks right? #Speaker: Emerald
                yeah, when ceramic breaks and is repaired with gold. I would tell them if we can endure the hard moments we can come out even better on the other side #Speaker: Amika
                that’s a beautiful thought #Speaker: Emerald
                do you really believe it? #Speaker: Emerald
                I mean I want to. I hope becoming an adult is like that #Speaker: Amika

        **  it’s relatable #Speaker: Amika
                yeah it resonated with me too #Speaker: Emerald
                I wouldn’t have guessed that - you always seem so confident to me #Speaker: Amika
                well fake it till you make it right? #Speaker: Emerald
                I don’t know how much longer I can fake for #Speaker: Amika
                
*   “following the moonlight where she goes” #Speaker: Amika
        ha! figures you’d pick the line about the moon XD #Speaker: Emerald
        omg called out #Speaker: Amika
        I’m literally wearing my moon hoodie right now [emoji:joy] needed to feel some comfort #Speaker: Amika
        aw how come? #Speaker: Emerald
        
        **  I'm anxious about work #Speaker: Amika
                I've got a performance review coming up [emoji:grimace] #Speaker: Amika
                no, don’t be anxious!! #Speaker: Emerald
                you’re amazing, I know you’ll do great [emoji:heart] #Speaker: Emerald
                just think about that big fat raise you’ll have in your pocket when you’re done ;) #Speaker: Emerald
                aw, thanks!! Here’s hoping lol [emoji:crossed-fingers] #Speaker: Amika
                
        **  just to be comfy! #Speaker: Amika
                ah, ok! Yeah, it always looked super-cozy #Speaker: Emerald
                i’m rocking a pair of comfy sweatpants right now [emoji:sunglasses-face] #Speaker: Emerald
                an Emerald classic XD #Speaker: Amika
                hell yeah lmao #Speaker: Emerald

        -- so are you feeling more like the moonlight or the follower in that line? #Speaker: Emerald
	   
        **   the light, just floating on #Speaker: Amika
	            like how do you mean? #Speaker: Emerald
	            
		        *** I keep moving forward #Speaker: Amika
                        I’m holding my head above water, just floating on ahead #Speaker: Amika
                		whatever comes my way, I’ve just gotta keep going #Speaker: Amika
                		it’s impressive that you’re able to do that #Speaker: Emerald
                		I guess so! It’s a handy mindset sometimes #Speaker: Amika
        		
		        ***  I’m floating aimlessly #Speaker: Amika
        		        I still have no idea where I’m going with life lol #Speaker: Amika
        		        honestly could have fooled me. It always seemed like you had a lot of direction #Speaker: Emerald
        		        like, going after animation, moving to Burbank, and everything #Speaker: Emerald
        		        i guess that’s true. Though sometimes I’m not sure where I’m supposed to end up #Speaker: Amika
		        
        **  I’m just the follower #Speaker: Amika
                I definitely need to take direction from someone else right now lol #Speaker: Amika
                I know what that feels like #Speaker: Emerald
                
        -- hey didn’t you need some comfort this week too? #Speaker: Amika
        wdym? #Speaker: Emerald
        you said you had wishing well on repeat because it was soothing right? everything ok? #Speaker: Amika
        oh yeah for sure!! not like I have an actual career to worry about like you [emoji:sweat_smile] #Speaker: Emerald
        
- you sound as stressed as the person in the song #Speaker: Emerald

*   I’m not that far gone! #Speaker: Amika
        I’ve still got some fight left in me yet, lol #Speaker: Amika
        that’s good! #Speaker: Emerald
        hopefully I’ll never get as down in the dumps as them, it sounds like they have it pretty rough #Speaker: Amika
        
*   it feels like it sometimes #Speaker: Amika

        {
            - player_said_song_gets_them:
        	    yeah I had a feeling when you said the song “gets you” lol #Speaker: Emerald
        	
        	- else:
        	    aw :( #Speaker: Emerald
        }

        honestly me too, tbh #Speaker: Emerald
        I think the song is about being stuck. Like, the person can’t really move forward or move on #Speaker: Emerald
        maybe. But they seem kind of lonely, too #Speaker: Amika
        
- they think they can walk through hell alone but maybe they don’t have to #Speaker: Amika
I don’t think they chose to be alone, though #Speaker: Emerald
I think they’ve been left behind #Speaker: Emerald
huh maybe. the end of the song does talk about waiting for someone #Speaker: Amika
yeah I think it’s easy to want someone else to fix you when you feel stuck without any help, right? #Speaker: Emerald
I think I get what you mean. To me the song is about #Speaker: Amika

*   independence #Speaker: Amika
        like, freeing yourself from unhelpful relationships #Speaker: Amika
        yeah sometimes it’s better to turn the page than to hold onto something that isn’t working #Speaker: Emerald
        
*   self-isolation #Speaker: Amika
        like, pushing people away and ending up with no one to help you #Speaker: Amika
        well don’t you think sometimes that’s the better option - to handle things on your own instead of bothering other people? #Speaker: Emerald
        
        **  totally #Speaker: Amika
                you’re the only person who can decide what’s best for your life #Speaker: Amika
        	    yeah, that’s what I feel #Speaker: Emerald
        	
        **  I’m not so sure #Speaker: Amika
                sometimes maybe, but other people can also offer an outside perspective you hadn’t considered before #Speaker: Amika
                I guess that’s true #Speaker: Emerald
        
*   trying to move on #Speaker: Amika
        the person is moving on from the past to an uncertain future #Speaker: Amika
        hmm that sounds nice #Speaker: Emerald
        
- what about your favorite lyric? #Speaker: Amika
probably “spend my time within a dream, I'll write a page for every scene” #Speaker: Emerald
oh that’s funny! this song was in my dream last night #Speaker: Amika
wait what kind of dream? #Speaker: Emerald
I listened to wishing well right before I fell asleep and then I had a weird dream about a black hole in space #Speaker: Amika
are you for real!? I had a dream like that too!!!! #Speaker: Emerald

*   what?? #Speaker: Amika
        how is that possible?? #Speaker: Amika
        you tell me! #Speaker: Emerald
        
*   huh, that’s weird #Speaker: Amika
        yeah, super-weird! #Speaker: Emerald
        how does that just happen?? #Speaker: Emerald
        I suppose it could be some kind of weird coincidence #Speaker: Amika
        
- were there texts in your dream? like SPEWING out of the black hole #Speaker: Amika
wait wouldn’t that make it a white hole [emoji:thinking_face] #Speaker: Amika
yes my dream was EXACTLY like that!! but idk anything about black holes or why I would dream about them #Speaker: Emerald

*   ok, now I’m freaking out! #Speaker: Amika
    	yeah like wth is going on!! #Speaker: Emerald
    	
*	there’s an explanation #Speaker: Amika
    	dude maybe we’re psychic [emoji:eyes] #Speaker: Emerald
    	well idk about that haha #Speaker: Amika
    	maybe it just sounds like we had the same dream because we each don’t remember it super-well #Speaker: Amika
    	so we’re filling in the gaps based on what the other person is saying #Speaker: Amika
    	I guess that could be it . . . #Speaker: Emerald
    	
*	this is so wacky haha #Speaker: Amika
	    yeah lol but isn’t it kind of crazy? #Speaker: Emerald
	    I guess so! Tbh I just find it kind of funny #Speaker: Amika
	    
- was there anything else in your dream? #Speaker: Emerald
Memi was following me around! #Speaker: Amika
another Memi sighting? sounds like you miss our lil guy [emoji:joy] #Speaker: Emerald

*   I totally do! #Speaker: Amika
        he was always so cute dancing along to your beats #Speaker: Amika
        aw man I still remember the dance he did in our first video. but I’m sure your art style has changed since high school #Speaker: Emerald
        oh gosh I haven’t looked at “Memories” in years! that was some of our best work #Speaker: Amika
        yeah we always made for a good team #Speaker: Emerald
        I miss that #Speaker: Emerald
        
*   idk if that’s it #Speaker: Amika
        besides, he was the one chasing me! #Speaker: Amika
        maybe he’s lonely then haha #Speaker: Emerald
        it’s easy to feel that way #Speaker: Emerald
        
- hey, Em? can I ask you something weird about the song you sent? #Speaker: Amika
yeah what do you want to know? #Speaker: Emerald
I’m just thinking about the person in the song and what they’re going through #Speaker: Amika
have you ever felt the same way as them? #Speaker: Amika
how so? #Speaker: Emerald
I guess like you’re . . . #Speaker: Amika

*   lonely? #Speaker: Amika
        sure I do. doesn’t everyone? #Speaker: Emerald
        I don’t mean the alone kind of lonely #Speaker: Amika
        I mean the deeper loneliness where you feel like you can’t reach out to anyone #Speaker: Amika
        do you ever feel that? #Speaker: Amika
        
*   really struggling? #Speaker: Amika
    	well sure. everyone could use a confidence boost once in a while #Speaker: Emerald
    	
    	{
    	    - player_opened_up_about_directing:
    	        I know, you gave me a huge one yesterday! #Speaker: Amika
    	}
    	
        you can always reach out to me when you feel that way. you know I’m still here whenever you need someone right? #Speaker: Amika
        “here” being across the country? #Speaker: Emerald
        jk lol #Speaker: Emerald
        Emerald do you think things would be better if I hadn’t moved away? #Speaker: Amika
        
- are you asking me if I feel abandoned? #Speaker: Emerald
would you really tell me if I didn’t ask? #Speaker: Amika
I mean I do feel alone a lot. I still think of you as my best friend but once we went to college we couldn’t talk and hang out every day like we used to #Speaker: Emerald

*   I didn’t realize that #Speaker: Amika
        oh I’m sorry Amika . . . I could have tried reaching out more too #Speaker: Emerald
        
*   I’m so sorry Emerald. #Speaker: Amika
        no no we’ve both had a lot of change this year #Speaker: Emerald
        I feel guilty that we haven’t talked in so long #Speaker: Amika
        
- when I got busy I just assumed you were living it up in college and booking shows and didn’t need me anymore. I should have asked you how you felt about that! #Speaker: Amika
no I totally get that you’ve got a lot going on! And to be honest part of why I’ve felt lonely is because school and music don’t take up any of my time anymore #Speaker: Emerald
Amika I have something to tell you too #Speaker: Emerald
I actually dropped out of music school #Speaker: Emerald
wait seriously? #Speaker: Amika
a couple semesters ago #Speaker: Emerald
that long ago?? #Speaker: Amika

*   I’ve missed a lot #Speaker: Amika

*   as long as you’re sure #Speaker: Amika
        if it’s what you wanted I guess it’s for the best? #Speaker: Amika
        thanks for understanding #Speaker: Emerald
        
*   that’s a huge decision! #Speaker: Amika
        I know. It was a really difficult one to make. #Speaker: Emerald
        
- it’s not as bad as it sounds. I promise. #Speaker: Emerald
I actually have to go now #Speaker: Emerald
Emerald wait can we keep talking for a few minutes #Speaker: Amika
I really have to head out. but here’s another nsa song for you, it’s called “Sleepless” #Speaker: Emerald
it’s one of my favorites #Speaker: Emerald
did I do something wrong? #Speaker: Amika
Emerald are you still there? #Speaker: Amika
->END
