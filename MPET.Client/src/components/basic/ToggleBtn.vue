<script setup>

const props = defineProps({
    modelValue: {type: Boolean, required: true},
    label: {type: String, required: true}
});

const emit = defineEmits('update:modelValue')

const swap = () => {
    emit('update:modelValue', !props.modelValue)
};

</script>

<template>
        
    <div class = "toggle-wrapper">
        <label>{{label}}</label>
        <div class="toggle-switch" @click="swap">
            <div :class="['track', modelValue ? 'on' : 'off']">
                <div :class="['knob', modelValue ? 'slide-on' : 'slide-off']"></div>
            </div>
        </div>
    </div>

</template>

<style scoped>

.toggle-wrapper{
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 5px;
}

.toggle-switch {
  width: 44px;
  height: 24px;
  display: flex;
  align-items: center;
  cursor: pointer;
}

.track {
  width: 100%;
  height: 8px;
  border-radius: 999px;
  position: relative;
  background-color: #9ca3af; /* grey off */
  transition: background-color 0.25s ease;
}

/* States */
.track.on {
  background-color: rgba(17, 170, 86, 0.5); /* green trail */
}

.track.off {
  background-color: #9ca3af;
}

.knob {
  width: 20px;
  height: 20px;
  background-color: #11aa56; /* primary green */
  border-radius: 50%;
  position: absolute;
  top: 50%;
  left: 0;
  transform: translate(0, -50%);
  transition: transform 0.25s ease, box-shadow 0.2s ease;
  box-shadow: 0 2px 6px rgba(0,0,0,0.2);
}

.slide-on {
  transform: translate(24px, -50%);
}

.slide-off {
  transform: translate(0, -50%);
}

label {
    font-size: 13px;
    font-weight: 500;
    color: #374151;
}


</style>