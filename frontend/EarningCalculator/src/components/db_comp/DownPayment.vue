<script setup>

import {ref} from 'vue'
import SingleInput from '../basic/SingleInput.vue';

const manualDownPayment = ref(false)

const prop = defineProps({
    manualDownPayment: {type: Boolean, required: true},
    modelValue: {type: Number, required: true}
});

const emit = defineEmits('update:modelValue', 'inputMethod')

const onInput = ( (event) => {
    const value = event.target.value;
    const parsed = value === "" ? null : Number(value);
    emit("update:modelValue", parsed);
    emit("inputMethod", manualDownPayment);
});

</script>

<template>

    <div class = "down-payment-div">
        <SingleInput label = "Down Payment Amount: " style = "no-border" :value = "modelValue" @input = "onInput"/>
        
        <div class = "toggle-wrapper">
            <label>Manually Input</label>
            <div class="toggle-switch" @click="manualDownPayment = !manualDownPayment">
                <div :class="['track', manualDownPayment ? 'on' : 'off']">
                    <div :class="['knob', manualDownPayment ? 'slide-on' : 'slide-off']"></div>
                </div>
            </div>
        </div>
    </div>

</template>

<style scoped>


.down-payment-div {
    display: flex;
    gap: 20px;
    padding: 12px 14px;
    border-radius: 12px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 2px 6px rgba(0,0,0,0.04);
    align-items: center;
}

.down-payment-div label {
    font-size: 13px;
    font-weight: 500;
    color: #374151;
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


</style>