window.audioPlayerInterop = {
    playAudio: audioElement => {
        audioElement.play();
    },
    pauseAudio: audioElement => {
        audioElement.pause();
    },
    stopAudio: audioElement => {
        audioElement.pause();
        audioElement.currentTime = 0;
    },
    isPlaying: audioElement => {
        return audioElement.paused === false;
    }
};