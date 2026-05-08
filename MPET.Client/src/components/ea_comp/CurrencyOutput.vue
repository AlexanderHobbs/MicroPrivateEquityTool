
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

    <div class = "data year"><img src = "../../assets/output-icons/calendar.png"/><span>Year: </span>
      <span class = "output" v-if = "label">{{label}}</span><span v-else> — </span></div>

    <div class = "data revenue"><img src = "../../assets/output-icons/moneyg.png"/><span>Revenue Amount: </span>
      <span class = "output" v-if = "data.revenue">{{ formatCurrency(data.revenue)}}</span><span v-else> — </span></div>

    <div class = "data expense"><img src = "../../assets/output-icons/moneyr.png"/><span>Expense Amount: </span>
      <span class = "output" v-if = "data.expense">{{ formatCurrency(data.expense) }}</span><span v-else> — </span></div>

    <div class = "data sde"><img src = "../../assets/output-icons/percent.png"/><span>Reported SDE Amount: </span>
      <span class = "output" v-if = "data.ReportedSDE">{{ formatCurrency(data.ReportedSDE) }}</span><span v-else> — </span></div>

    <div class = "data salary"><img src = "../../assets/output-icons/moneyb.png"/><span>Owner Salary Amount: </span>
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

.data.year {
  background-color: rgb(128, 128, 128, 0.06);
}

span {
  flex: 1;
}

.output {
  font-weight: 550;
}

.data.revenue {
  background-color: rgb(128, 128, 128, 0.06);
}

.data.revenue .output {
  /* color: #2E8B57 */
}

.data.expense {
  background-color: rgb(128, 128, 128, 0.06);
}

.data.expense .output {
  /* color:#B94A48; */
}

.data.sde {
  background-color: rgb(128, 128, 128, 0.06);
}

.data.sde .output {
  /* color: #B8963A; */
}

.data.salary {
  background-color: rgb(128, 128, 128, 0.06);
}

.data.salary .output {
  /* color: #3A5F8A; */
}


img {
    height: 40px;
    border-radius: 50px;
}


</style>