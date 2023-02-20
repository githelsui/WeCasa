import React from 'react';
import { Table, Button} from 'antd';

export const ButtonIcon = (props) => {
  	const parent = props;

	  const items = [
        {
          key: '1',
          label:  <b>Current</b>,
          children: <Table dataSource={parent.dataSource} columns={parent.columns} />,
        },
        {
          key: '2',
          label: <b>History</b>,
          children: 'History Table',
        }
      ];

  	let buttons = parent.readings && parent.readings.length && parent.readings.map(function(item, i) {
		return (
			<div className="button" style={{'color': item.color}}  key={i}>
				<Button shape="round" style={{ background: item.color }} size='large' ></Button>
			</div>
		)
  	}, this);

    return (
      	<div className="buttons">
      		{buttons == ''?'':buttons}
      	</div>
    );
}

export default ButtonIcon;

