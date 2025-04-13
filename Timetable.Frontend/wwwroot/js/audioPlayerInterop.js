window.audioPlayerInterop = {
    playAudio: audioElement => {
        audioElement.play();
    },
    pauseAudio: audioElement => {
        audioElement.pause();
        audioElement.currentTime = 0;
    }
};