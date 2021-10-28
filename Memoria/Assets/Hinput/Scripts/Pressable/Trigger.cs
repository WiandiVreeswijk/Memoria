using HinputClasses.Internal;
using UnityEngine;

namespace HinputClasses {
    /// <summary>
    /// Hinput class representing the left or right trigger of a controller.
    /// </summary>
    public class Trigger : GamepadPressable {
        // --------------------
    	// CONSTRUCTOR
    	// --------------------
    
    	public Trigger (string pressableName, Gamepad gamepad, int index, bool isEnabled) : 
    		base(pressableName, gamepad, index, isEnabled) {
    		initialValue = measuredPosition;
    	}
    
    	
    	// --------------------
    	// UPDATE
    	// --------------------
    
        public override void Update() {
	        base.Update();

	        position = GetPosition();
        }

        private readonly float initialValue;
        private bool hasBeenMoved = false;
    
        // In some instances, triggers have a non-zero resting position until an input is recorded.
        private float GetPosition() {
	        if (!hasBeenMoved) {
		        if (measuredPosition.IsEqualTo(initialValue)) return 0;
		        else hasBeenMoved = true;
	        }
	        
	        if (Settings.triggerDeadZone.IsEqualTo(1)) return 0;
	        return Mathf.Clamp01((measuredPosition - Settings.triggerDeadZone)/(1 - Settings.triggerDeadZone));
        }
        
        // The value of the trigger's position, measured by the gamepad driver.
        private float measuredPosition { 
	        get { // Triggers range from -1 to 1 on Mac, and from 0 to 1 on Windows and Linux.
		        if (Internal.Utils.os == Internal.Utils.OS.Mac) return (Internal.Utils.GetAxis(Internal.Utils.os + "_" + name) + 1)/2;
		        else return Internal.Utils.GetAxis(Internal.Utils.os + "_" + name);	
	        }
        }

        protected override bool GetPressed() { return position >= Settings.triggerPressedZone; }

		
        // --------------------
        // PUBLIC PROPERTY
        // --------------------
        
        /// <summary>
        /// The position of a trigger, between 0 and 1.
        /// </summary>
        public float position { get; private set; }
    }
}