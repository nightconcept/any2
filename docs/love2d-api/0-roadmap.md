# LÖVE 2D Implementation Roadmap

## Version 0.1.0 (Default)

### Modules

- [ ] `love.audio`: Provides an audio interface for playback/recording sound.

- [ ] `love.filesystem`: Provides an interface to the user's filesystem.

- [ ] `love.graphics`: Drawing of shapes and images, management of screen geometry.

- [ ] `love.image`: Provides an interface to decode encoded image data.

- [ ] `love.keyboard`: Provides an interface to the user's keyboard.

- [ ] `love.mouse`: Provides an interface to the user's mouse.

- [ ] `love.sound`: This module is responsible for decoding sound files.

- [ ] `love.timer`: Provides high-resolution timing functionality.

### Callbacks - General

- [ ] `love.draw`: Callback function used to draw on the screen every frame.

- [ ] `love.load`: This function is called exactly once at the beginning of the game.

- [ ] `love.run`: The main callback function, containing the main loop. A sensible default is used when left out.

- [ ] `love.update`: Callback function used to update the state of the game every frame.

### Callbacks - Keyboard

- [ ] `love.keypressed`: Callback function triggered when a key is pressed.

- [ ] `love.keyreleased`: Callback function triggered when a keyboard key is released.

### Callbacks - Mouse

- [ ] `love.mousepressed`: Callback function triggered when a mouse button is pressed.

- [ ] `love.mousereleased`: Callback function triggered when a mouse button is released.

### Callbacks - Joystick

- [ ] `love.joystickpressed`: Callback function triggered when a joystick button is pressed.

- [ ] `love.joystickreleased`: Callback function triggered when a joystick button is released.

### Types

- [ ] `Data`: The superclass of all data.

- [ ] `Object`: The superclass of all LÖVE types.

- [ ] `Variant`: The types supported by love.thread and love.event.

### General

- [ ] Config Files: Game configuration settings.

## Version 0.4.0

### Modules

- [ ] `love.physics`: Can simulate 2D rigid body physics in a realistic manner.

## Version 0.5.0

### Modules

- [ ] `love.joystick`: Provides an interface to connected joysticks.

### Third-party modules

- [ ] `socket`: Module for HTTP, TCP, and UDP networking.

## Version 0.6.0

### Modules

- [ ] `love.event`: Manages events, like keypresses.

## Version 0.7.0

### Modules

- [ ] `love.font`: Allows you to work with fonts.

- [ ] `love.thread`: Allows you to work with threads.

### Callbacks - General

- [ ] `love.quit`: Callback function triggered when the game is closed.

### Callbacks - Window

- [ ] `love.focus`: Callback function triggered when window receives or loses focus.

## Version 0.9.0

### Modules

- [ ] `love.math`: Provides system-independent mathematical functions.

- [ ] `love.system`: Provides access to information about the user's system.

- [ ] `love.window`: Provides an interface for the program's window.

### Third-party modules

- [ ] `lua-enet`: Multiplayer networking module for games.

### Callbacks - General

- [ ] `love.threaderror`: Callback function triggered when a Thread encounters an error.

### Callbacks - Window

- [ ] `love.mousefocus`: Callback function triggered when window receives or loses mouse focus.

- [ ] `love.resize`: Called when the window is resized.

- [ ] `love.visible`: Callback function triggered when window is shown or hidden.

### Callbacks - Keyboard

- [ ] `love.textinput`: Called when text has been entered by the user.

### Callbacks - Joystick

- [ ] `love.gamepadaxis`: Called when a Joystick's virtual gamepad axis is moved.

- [ ] `love.gamepadpressed`: Called when a Joystick's virtual gamepad button is pressed.

- [ ] `love.gamepadreleased`: Called when a Joystick's virtual gamepad button is released.

- [ ] `love.joystickadded`: Called when a Joystick is connected.

- [ ] `love.joystickaxis`: Called when a joystick axis moves.

- [ ] `love.joystickhat`: Called when a joystick hat direction changes.

- [ ] `love.joystickremoved`: Called when a Joystick is disconnected.

## Version 0.9.1

### Functions

- [ ] `love.getVersion`: Gets the current running version of LÖVE.

## Version 0.9.2

### Third-party modules

- [ ] `utf8`: Provides basic support for manipulating UTF-8 strings.

### Callbacks - Mouse

- [ ] `love.mousemoved`: Callback function triggered when the mouse is moved.

## Version 0.10.0

### Modules

- [ ] `love.touch`: Provides an interface to touch-screen presses.

- [ ] `love.video`: This module is responsible for decoding and streaming video files.

### Functions

- [ ] `love.isVersionCompatible`: Gets whether the given version is compatible with the current running version of LÖVE.

### Callbacks - General

- [ ] `love.lowmemory`: Callback function triggered when the system is running out of memory on mobile devices.

### Callbacks - Window

- [ ] `love.directorydropped`: Callback function triggered when a directory is dragged and dropped onto the window.

- [ ] `love.filedropped`: Callback function triggered when a file is dragged and dropped onto the window.

### Callbacks - Keyboard

- [ ] `love.textedited`: Called when the candidate text for an IME has changed.

### Callbacks - Mouse

- [ ] `love.wheelmoved`: Callback function triggered when the mouse wheel is moved.

### Callbacks - Touch

- [ ] `love.touchmoved`: Callback function triggered when a touch press moves inside the touch screen.

- [ ] `love.touchpressed`: Callback function triggered when the touch screen is touched.

- [ ] `love.touchreleased`: Callback function triggered when the touch screen stops being touched.

## Version 11.0

### Modules

- [ ] `love.data`: Provides functionality for creating and transforming data.

### Functions

- [ ] `love.hasDeprecationOutput`: Gets whether LÖVE displays warnings when using deprecated functionality.

- [ ] `love.setDeprecationOutput`: Sets whether LÖVE displays warnings when using deprecated functionality.

### Callbacks - General

- [ ] `love.errhand`: The error handler, used to display error messages. (Note: `love.errorhandler` is also listed for 11.0, likely an alias or the preferred name)

- [ ] `love.errorhandler`: The error handler, used to display error messages.

## Version 11.3

### Callbacks - Window

- [ ] `love.displayrotated`: Called when the device display orientation changed.
