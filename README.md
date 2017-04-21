# Facebook-Messenger-Export

## Purpose
A C# project to take the message export Facebook gives and turn it into something more machine readable. Might extend to creating a frontend for reading them. Maybe. 


## Hierarchy
* Start in the div `.contents`
* It apparently has several divs in it for some fucking reason (probably related to health of the DOM)
* Each chat has a div for it with class `.thread`
    * For some fucking reason this starts with a basic plain string (no `<p>` tag) that has a list of all users
* It then continues with a bunch of divs with class `.message`.
    * Spoiler: these don't fucking include the messages
    * Instead they contain a div of class `.message_header`
    * Within this div, there's 2 spans. One with class `.user` and the other with class `.meta`
        * User has the user's information. Sometimes this takes the form of a real name. Other times this takes the form of ID@facebook.com. Going to facebook.com/ID will take you to their profile. 
        * Meta has the date in this format: Monday, December 12, 2016 at 1:51pm CST
* *After* the div of class `.message`, there's a (classless) `<p>` tag with the actual plaintext of the mssage
    * I'm pretty convinced images aren't here at all.
    * Can't blame Facebook for that one - they can't expose them to the world via their CDN and the processing (+ raw size) of the export would be ridiculous if they were included
        * Still would like links that could be accessed by people with actual auth :/



## License
MIT