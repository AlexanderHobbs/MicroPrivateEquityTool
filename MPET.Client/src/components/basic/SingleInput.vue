<!-- CurrencyInput.vue -->

<script setup>

const prop = defineProps({
    label: {type: String, required: true},
    modelValue: {type: [Number, String, null], default: null},
    placeholder: {type: String, default: "00.00"},
    inputType: {type: Number, default: 1},
    class: {type: String, default: "single-input"},
    icon: {type: Image, default: "/src/assets/navbar-icons/break-even.png"}
});

const emit = defineEmits(['update:modelValue']);

const onInput = (event) => {
    const value = event.target.value;

    if(prop.inputType === 1){
        const parsed = value === "" ? null : Number(value);
        emit("update:modelValue", parsed);
    }else{
        emit("update:modelValue", value);
    }

}

// const nonDigit = ref()
// const checkDigit = (event) => {
//     if (isNaN(Number(event.key))) {
//         nonDigit = false
//         event.preventDefault(); // Stop the character from being entered
//     }else{
//         nonDigit = true
//     }
// }
</script>

<template>
  <div :class = "class" v-if = "inputType === 1" >
    <div class = "label"><label>{{ label }}</label></div>
    <input type="number" :placeholder= "placeholder" :value="modelValue" @input="onInput" />
  </div>

  <div :class = "class" v-if = "inputType === 2" >
    <label>{{ label }}</label>
    <input type="text" :value="modelValue" @input="onInput" />
  </div>

  <div :class = "class" v-if = "inputType === 3" >
    <label>{{ label }}</label>
    <textarea :value = "modelValue" @input="onInput"></textarea>
  </div>
  

    <!-- <Teleport to = "body">
        <div v-show = "nonDigit" class = "error-character">
            <p>Please enter a number</p>
            <button @click="nonDigit = true">Close</button>
        </div>
    </Teleport> -->
</template>


<style scoped>
.single-input {
    display: flex;
    align-items: center;
    gap: 12px;
    background: #ffffff;
}

.label {
    flex: 1;
    min-width: max-content;
    display: flex;
    align-items: center;
    gap: 20px;
}

.single-input input {
    flex: 1;
}

img {
    height: 20px;
    width: auto;
    border-radius: 7px;
    background-color: #f1f2f2;
    padding: 8px;
}

.no-border {
    display: flex;
    align-items: center;
    gap: 60px;
}

label {
    font-size: 16px;
    font-weight: 500;
    color: #6b7280;
}

input,
textarea {
    flex: 2;
    padding: 15px 10px;
    border-radius: 6px;
    border: 1px solid #e6e8ec;
    outline: none;
    font-size: 13px;
    background: #f9fafb;
    transition: all 0.15s ease;
    font-family: inherit;
}

input::placeholder {
    color: #9ca3af;
}

input:hover,
textarea:hover {
    background: #f3f4f6;
}

input:focus,
textarea:focus {
    background: #ffffff;
    border-color: #d1d5db;
    box-shadow: 0 0 0 1px rgba(17, 24, 39, 0.05);
}
</style>