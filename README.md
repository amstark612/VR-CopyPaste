# VRCutPaste

**Versions used for AR-CutPaste:**  
Python 3.7.0  
NPM 6.14.5  
Node.js 14.4.0  
Expo CLI 3.21.10  
Flask 1.1.1  
  
**AR-CutPaste setup:**  
Change port in *ar-cutpaste/server/src/main.py* on line 122 in addition to instructions in READMEs in the following folders:
-*ar-cutpaste*
-*ar-cutpaste/server*
-*ar-cutpaste/app*

**Unity setup**
Change local file path in inspector for ImageMenu object

**Notes on user interaction**
Ray interaction:
-Depress and hold index trigger to activate raycast line renderer
-Depress and hold hand trigger to grab object
-Thumbstick up and down to push/pull object farther/closer
-*Debating eliminating other interaction mode entirely*
-*Debating to eliminate index trigger and have raycast line renderer enabled at all times*

Bimanual scaling:
-OVRPlayerController needs to be on a special layer that cannot interact with Grabbables layer, otherwise scaling large objects will collide with player capsule collider and push player backward (*fix by calculating offset?*)

**To Do**
Unity:
-Implement bimanual scaling using ray interaction
-Implement bimanual rotation
-Implement rotation snapping
-Implement snapping to ground
-Implement some kind of hemisphere/dome as a sky to which objects can be snapped
-Implement a refresh button for UI to load new images
-Add limits to scaling too large/small
-Add a linear function for increasing speed when pushing/pulling objects at great distances

Mobile app:
-Trim transparent pixels from captured images
-Choose whether to keep or remove background
-View images you've already taken
