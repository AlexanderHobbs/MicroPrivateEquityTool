
<script setup>


const props = defineProps({
    label: { type: String, required: true },
    data: { type: Object, default: null}
});

const currencyFormatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
});

const formatCurrency = (value) => {
    if(value === null || value === undefined) return '-';
  return currencyFormatter.format(value);
};

</script>

<template>

  <div class = "output-container">

    <div class = "data year"><img src = "../../assets/navbar-icons/earning.png"/><span>Year: </span>
      <span class = "output" v-if = "label">{{label}}</span><span v-else> — </span></div>

    <div class = "data revenue"><img src = "../../assets/navbar-icons/earning.png"/><span>Revenue Amount: </span>
      <span class = "output" v-if = "data.revenue">{{ formatCurrency(data.revenue)}}</span><span v-else> — </span></div>

    <div class = "data expense"><img src = "../../assets/navbar-icons/earning.png"/><span>Expense Amount: </span>
      <span class = "output" v-if = "data.expense">{{ formatCurrency(data.expense) }}</span><span v-else> — </span></div>

    <div class = "data sde"><img src = "../../assets/navbar-icons/earning.png"/><span>Reported SDE Amount: </span>
      <span class = "output" v-if = "data.ReportedSDE">{{ formatCurrency(data.ReportedSDE) }}</span><span v-else> — </span></div>

    <div class = "data salary"><img src = "../../assets/navbar-icons/earning.png"/><span>Owner Salary Amount: </span>
      <span class = "output" v-if = "data.ownerSalary">{{ formatCurrency(data.ownerSalary) }}</span><span v-else> — </span></div>
  </div>

</template>


<style scoped>

.output-container {
    display: flex;
    flex-direction: column;
    gap: 15px;
    margin-bottom: 10px;
}

.data {
  display: flex;
  gap: 10px;
  align-items: center;
  border-radius: 15px;
}


span {
  flex: 1;
}

.output {
  font-weight: 550;
}


img {
    height: 30px;
    border-radius: 50px;
    padding: 5px;
    background-color: rgb(128, 128, 128, 0.06);
}


</style>